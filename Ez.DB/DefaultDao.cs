using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.DBContract;
using System.Data.Common;
using System.Data;
using System.Reflection;
using Ez.Core;
using System.IO;
using System.Collections;
using Ez.Core.Attributes;
using System.Linq.Expressions;
using System.ComponentModel;
using Ez.Core.Interceptor;
namespace Ez.DB
{
    /// <summary>
    /// 数据库持久层
    /// </summary>
    public partial class DefaultDao : BaseDao, IDefaultDao
    {
        /// <summary>
        /// 构造器，出发数据库配置信息的生成
        /// <param name="scope">指定索引</param>
        /// <param name="pluginName">插件名,不存在是表示为宿主的数据库处理器</param>
        /// </summary>
        public DefaultDao(string scope, string pluginName = null)
            : base(scope, pluginName)
        {

        }

        public IDefaultDao GetInstance(string scope)
        {
            return new DefaultDao(scope, this.PluginName);
        }
        /// <summary>
        /// 获取最大编号,不需要进行+1操作
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public int GetMaxID(string tableName, string fieldName = "ID")
        {
            string strsql = "select max(" + fieldName + ")+1 from " + tableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 获取业务序号信息
        /// </summary>
        /// <param name="key">计数器的key</param>
        /// <returns></returns>
        public FW_Squence GetSequenceEntity(string key)
        {
            int proid = ConnectMaster.Proid;
            if (string.IsNullOrEmpty(key)) throw new Exception("获取业务序列时表名不允许为空！");
            if (proid <= 0) throw new Exception("获取业务序列时产品编号不允许为空！");

            string sql = "";

            if (this.ConnectionEntity.Dbtype == DBTypeEnum.MySql)
            {
                sql = "select id,pro_id,skey,prefix,svalue,min_len,supple_char,ext_realize_dll from FW_Squence where skey=@key and pro_id=@pro_id LIMIT 0,1";
            }
            else
            {
                sql = "select top 1 id,pro_id,skey,prefix,svalue,min_len,supple_char,ext_realize_dll from FW_Squence where skey=@key and pro_id=@pro_id";
            }

            bool isFirstRecord = false;
            FW_Squence entity = this.GetEntity<FW_Squence>(sql, new DbParam("@key", key), new DbParam("@pro_id", proid));
            if (isFirstRecord = (entity == null))
            {
                //CODING... 统计表中的记录数+1 设置计数器表的累计值
                if (this.ExecuteSql("insert into FW_Squence(pro_id,skey,svalue) values(@pro_id,@key,1)",
                    new DbParam("@pro_id", proid), new DbParam("@key", key)) > 0)
                {
                    entity = new FW_Squence
                    {
                        pro_id = proid,
                        svalue = 1,
                        skey = key
                    };
                }
            }
            if (entity == null)
            {
                throw new Exception("未能从数据库中获取" + key + "的业务序列！");
            }
            else if (!isFirstRecord && this.ExecuteSql("update FW_Squence set svalue=svalue+1 where skey=@key and pro_id=@pro_id",
                    new DbParam("@pro_id", proid), new DbParam("@key", key)) <= 0)
            {
                throw new Exception("更新数据库中" + key + "的业务序列失败！");
            }
            return entity;
        }
        /// <summary>
        /// 获取业务序号的值
        /// </summary>
        /// <param name="key">计数器的key</param>
        /// <returns></returns>
        public int GetSequenceVal(string key)
        {
            FW_Squence entity = GetSequenceEntity(key);
            if (entity != null)
            {
                return entity.svalue;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取业务序号
        /// </summary>
        /// <param name="key">计数器的key</param>
        /// <returns></returns>
        public string GetSequence(string key)
        {
            string sequence = "";
            FW_Squence entity = GetSequenceEntity(key);
            if (entity != null)
            {
                if (string.IsNullOrEmpty(entity.ext_realize_dll))
                {
                    sequence = entity.svalue.ToString();
                    if (entity.min_len > 0 && sequence.Length < entity.min_len)
                    {
                        int dif = entity.min_len - sequence.Length;
                        string _char = string.IsNullOrEmpty(entity.supple_char) ? "0" : entity.supple_char;
                        StringBuilder append = new StringBuilder();
                        for (int i = 0; i < dif; i++) append.Append(_char);
                        sequence = append + sequence;
                    }
                    if (!string.IsNullOrEmpty(entity.prefix)) sequence = entity.prefix + sequence;

                }
                else
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, entity.ext_realize_dll));
                        Type curenttype = assembly.GetTypes().FirstOrDefault(p => typeof(IBizSequence).IsAssignableFrom(p));
                        if (curenttype != null)
                        {
                            IBizSequence instance = (IBizSequence)assembly.CreateInstance(curenttype.FullName);
                            if (instance == null)
                            {
                                throw new Exception("用于生成业务序列的实现" + entity.ext_realize_dll + "可能为实现要求的接口：Ez.BizContract.IBizSequence!");
                            }
                            else
                            {
                                sequence = instance.GetSequence(entity.svalue, entity.prefix, entity.min_len, entity.supple_char);
                            }
                        }
                        else
                        {
                            throw new Exception("用于生成业务序列的实现" + entity.ext_realize_dll + "可能为实现要求的接口：Ez.BizContract.IBizSequence!");
                        }
                    }
                    catch (Exception exp)
                    {
                        throw new Exception("用于生成业务序列的实现" + entity.ext_realize_dll + "不存在!" + exp.Message);
                    }
                }
            }
            return sequence;
        }
        /// <summary>
        /// 检测是否存在
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Exists(string sqlString, params DbParam[] parameters)
        {
            object obj = GetSingle(sqlString, parameters);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult != 0;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询模型</param>
        /// <param name="records">记录数</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public IList<T> QueryPaging<T>(QuerySql query, out int records, params DbParam[] parameters) where T : class,new()
        {
            records = 0;
            DataSet ds = null;
            string sqlstring = query.ToString(this.ConnectionEntity.Dbtype);
            Console.WriteLine(sqlstring);
            IList<T> list = GetEntities<T>(sqlstring, out ds, parameters);
            if (ds != null && ds.Tables.Count == 2)
            {
                object obj_records = ds.Tables[1].Rows[0][0];
                records = obj_records.ToSafeInt();
            }
            return list;
        }
        /// <summary>
        /// 获取查询的实体信息,要求 查询结果的列名与T实例的属性名一致，可以通过 select login as loginName from ...形式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public T GetEntity<T>(string sqlString, params DbParam[] parameters) where T : class,new()
        {
            DataSet ds = this.Query(sqlString, parameters);
            T t = default(T);

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                t = InstallEntity<T>(ds.Tables[0].Columns, ds.Tables[0].Rows[0]);
            }
            return t;
        }
        /// <summary>
        /// 查询数据的实体集合,要求 查询结果的列名与T实例的属性名一致，可以通过 select login as loginName from ...形式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public IList<T> GetEntities<T>(string sqlString, params DbParam[] parameters) where T : class,new()
        {
            DataSet ds = null;
            return GetEntities<T>(sqlString, out ds, parameters);
        }
        /// <summary>
        /// 获取单项数据结合，集合元素类型必须为基本数据类型,待测试
        /// </summary>
        /// <typeparam name="T">基本数据类型</typeparam>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public IList<T> GetList<T>(string sqlString, params DbParam[] parameters)
        {
            TypeCode targetType = Type.GetTypeCode(typeof(T));
            if (targetType == TypeCode.String || targetType == TypeCode.Int32 || targetType == TypeCode.Int64)
            {
                IList<T> list = new List<T>();
                DataSet ds = this.Query(sqlString, parameters);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (targetType == TypeCode.String || System.Text.RegularExpressions.Regex.IsMatch(row[0].ToSafeString(), @"^[0-9]$"))
                        {
                            list.Add((T)row[0]);
                        }
                    }
                }
                return list;
            }
            else
                return new List<T>();
        }
        /// <summary>
        /// 查询数据返回array列表
        /// </summary>
        /// <param name="sqlString">基本数据类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public IList<object> GetDataArray(string sqlString, params DbParam[] parameters)
        {
            IList<object> list = new List<object>();
            DataSet ds = this.Query(sqlString, parameters);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                int col_num = ds.Tables[0].Columns.Count;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    object[] array = new object[col_num];
                    for (int i = 0; i < col_num; i++)
                    {
                        array[i] = row[i];
                    }
                    list.Add(array);
                }
            }
            return list;
        }

        private T InstallEntity<T>(DataColumnCollection dcc, DataRow row) where T : class,new()
        {
            T t = default(T);
            if (row != null)
            {
                t = new T();
                PropertyInfo[] ps = t.GetType().GetProperties();
                foreach (DataColumn column in dcc)
                {
                    PropertyInfo thisproto = ps.FirstOrDefault(p => p.Name.ToLower().Equals(column.ColumnName.ToLower()) && p.MemberType == MemberTypes.Property);
                    if (thisproto != null)
                    {
                        SetValue(t, thisproto, row[column.ColumnName]);
                    }
                }
            }
            return t;
        }
        private IList<T> GetEntities<T>(string sqlString, out DataSet ds, params DbParam[] parameters) where T : class,new()
        {
            IList<T> list = new List<T>();
            ds = this.Query(sqlString, parameters);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataColumnCollection dcc = ds.Tables[0].Columns;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    T t = InstallEntity<T>(dcc, row);
                    list.Add(t);
                }
            }
            return list;
        }
        private void SetValue(object obj, PropertyInfo thisproto, object columnvalue)
        {
            TypeCode tcode = Type.GetTypeCode(thisproto.PropertyType);
            try
            {
                if (System.DBNull.Value != columnvalue)
                {
                    if (this.ConnectionEntity.Dbtype == DBTypeEnum.MySql)
                    {
                        if (tcode == TypeCode.DateTime && columnvalue.GetType().Name == "MySqlDateTime")
                        {
                            MySql.Data.Types.MySqlDateTime mysqldate = (MySql.Data.Types.MySqlDateTime)columnvalue;
                            columnvalue = new DateTime(mysqldate.Year, mysqldate.Month, mysqldate.Day, mysqldate.Hour, mysqldate.Minute, mysqldate.Second, mysqldate.Millisecond);
                        }
                        else if (tcode == TypeCode.Boolean)
                        {
                            columnvalue = columnvalue.ToSafeInt(0) > 0;
                        }
                        //else if (tcode == TypeCode.Int32 && columnvalue.GetType().Name=="Int64")
                        //{ 

                        //}
                    }
                    thisproto.SetValue(obj, columnvalue, null);
                }
            }
            catch (Exception exp)
            {
                Ez.Core.Interceptor.Log4NetManager.Output(new Core.Interceptor.ExecuteInfo
                {
                    TargetType = thisproto.MemberType.GetType(),
                    Exception = exp,
                    LogLevel = Core.Interceptor.LogLevel.Error,
                    MethodName = thisproto.Name + ".SetValue(...) from :" + obj.ToString() + "->" + columnvalue
                });
            }

        }

    }
    public partial class DefaultDao
    {
        /// <summary>
        /// 数据库操作方式
        /// </summary>
        private enum DBAction
        {
            /// <summary>
            /// 查询
            /// </summary>
            Select,
            /// <summary>
            /// 创建
            /// </summary>
            Create,
            /// <summary>
            /// 更新
            /// </summary>
            Update,
            /// <summary>
            /// 删除
            /// </summary>
            Delete,
            /// <summary>
            /// 创建或更新
            /// </summary>
            CreateOrUpdate
        }
        #region Private Build sql and parameters from entity
        /// <summary>
        /// 构建从映射生成的操作语句
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="DbAction">操作类型</param>
        /// <param name="ignoreEmpty">是否忽略位置的操作</param>
        /// <param name="parameters">执行所需的参数</param>
        /// <param name="watcher">监事的累的数据，相同更新不同新增</param>
        /// <returns></returns>
        private string BuildSqlAndParams<T>(T entity, DBAction DbAction, bool ignoreEmpty, out IList<DbParam> parameters, params string[] watcher)
        {
            Type entityType = entity.GetType();
            parameters = new List<DbParam>();
            object[] tableattr = entityType.GetCustomAttributes(typeof(AsTableAttribute), false);
            if (tableattr == null || tableattr.Length == 0) return null;
            AsTableAttribute tableclass = tableattr[0] as AsTableAttribute;
            string tableName = string.Empty;
            if (tableclass != null && !string.IsNullOrEmpty(tableclass.TableName))
            {
                tableName = tableclass.TableName;
            }
            else
            {
                tableName = entityType.Name;
            }

            PropertyInfo[] properties = entityType.GetProperties();
            IDictionary<string, object> primaryfields_dc = new Dictionary<string, object>();
            IList<DbParam> primaryfield_parameters = new List<DbParam>();
            IDictionary<string, object> fields_dc = new Dictionary<string, object>();
            if (properties != null)
            {
                foreach (PropertyInfo prop in properties)
                {
                    object pvalue = prop.GetValue(entity, null);
                    object[] proper = prop.GetCustomAttributes(typeof(AsFieldAttribute), false);
                    object[] ignore_properties = prop.GetCustomAttributes(typeof(IgnoreFieldAttribute), false);
                    if (ignore_properties != null && ignore_properties.Length == 1) continue;
                    if (pvalue == null && ignoreEmpty) { continue; }
                    else {

                        if (Type.GetTypeCode(prop.PropertyType) == TypeCode.String && pvalue == null)
                        {
                            pvalue = "";
                        }
                    }
                    string fieldName = string.Empty;
                    AsFieldAttribute fieldattr = null;
                    if (proper == null || proper.Length == 0)
                    {
                        fieldattr = new AsFieldAttribute
                        {
                            FieldName = prop.Name
                        };
                    }
                    else
                    {
                        fieldattr = proper[0] as AsFieldAttribute;
                    }
                    fieldName = (fieldattr != null && !string.IsNullOrEmpty(fieldattr.FieldName)) ? fieldattr.FieldName : prop.Name;
                    if ((!fieldattr.Primary || (fieldattr.Primary && !fieldattr.Auto)))
                    {
                        fields_dc.Add(fieldName, pvalue);
                    }
                    if (fieldattr.Primary)
                    {
                        primaryfields_dc.Add(fieldName, pvalue);
                    }
                }
            }

            string sqlstring = string.Empty;
            switch (DbAction)
            {
                case DBAction.Select:
                    sqlstring = SelectSqlString(tableName, fields_dc, primaryfields_dc, ref parameters);
                    break;
                case DBAction.Create:
                    sqlstring = CreateSqlString(tableName, fields_dc, ref parameters);
                    break;
                case DBAction.Delete:
                    sqlstring = DeleteSqlString(tableName, fields_dc, primaryfields_dc,ref parameters, watcher);
                    break;
                case DBAction.Update:
                    sqlstring = UpdateSqlString(tableName, fields_dc, primaryfields_dc,ref parameters, watcher);
                    break;
                case DBAction.CreateOrUpdate:
                    if (primaryfields_dc == null || primaryfields_dc.Count() == 0)
                    {
                        throw new Exception("创建或更新语句需要设置主键存在的条件才能执行！");
                    }
                    else
                    {
                        sqlstring = CreateOrUpdateSqlString(tableName, fields_dc, primaryfields_dc, ref parameters, watcher);
                    }
                    break;
                default: throw new Exception("为明确指出要操作的类型！");
            }

            return sqlstring;
        }
        /// <summary>
        /// 查询SQL语句
        /// </summary>
        private string SelectSqlString(string tableName, IDictionary<string, object> fields_dc, IDictionary<string, object> primaryfield, ref IList<DbParam> parameters)
        {
            if (primaryfield == null || primaryfield.Count() == 0)
            {
                throw new Exception("删除语句需要设置主键存在的条件才能执行！");
            }
            else
            {
                StringBuilder sqlbuilder = new StringBuilder("select ");
                int counter = 0;
                foreach (string key in fields_dc.Keys)
                {

                    sqlbuilder.AppendFormat("{0}{1}", key, (counter == fields_dc.Count ? ")  " : ","));
                    counter++;
                }
                sqlbuilder.AppendFormat(" from {0} where ", tableName);

                foreach (var pfiled in primaryfield)
                {
                    sqlbuilder.AppendFormat("{0}=@{0} and ", pfiled);
                }
                sqlbuilder.Append(" 1=1 ;");

                return sqlbuilder.ToString();
            }
        }
        /// <summary>
        /// 创建SQL语句
        /// </summary>
        private string CreateSqlString(string tableName, IDictionary<string, object> fields_dc,ref IList<DbParam> parameters)
        {
            parameters.Clear();
            StringBuilder sqlbuilder_pre = new StringBuilder("insert into " + tableName + "(");
            StringBuilder sqlbuilder_last = new StringBuilder(" values(");
            int counter = 0;
            foreach (string key in fields_dc.Keys)
            {
                counter++;
                sqlbuilder_pre.AppendFormat("{0}{1}", key, (counter == fields_dc.Count ? ")  " : ","));
                sqlbuilder_last.AppendFormat("@{0}{1}", key, (counter == fields_dc.Count ? ");  " : ","));
                parameters.Add(new DbParam("@" + key, fields_dc[key]));
            }
            return sqlbuilder_pre.Append(sqlbuilder_last).ToString();
        }

        /// <summary>
        /// 创建更新语句
        /// </summary>
        private string UpdateSqlString(string tableName, IDictionary<string, object> fields_dc, IDictionary<string, object> primaryfield_dc, ref IList<DbParam> parameters, string[] watcher)
        {
            parameters.Clear();
            IList<string> withOutfield = new List<string>();
            if (watcher != null && watcher.Length > 0)
            {
                foreach (string key in primaryfield_dc.Keys)
                {
                    withOutfield.Add(key);
                }
                IDictionary<string, object> _primaryfield_dc = new Dictionary<string, object>();
               
                foreach (string field in watcher)
                {
                    if (fields_dc.ContainsKey(field))
                    {
                        _primaryfield_dc.Add(field, fields_dc[field]);
                    }
                    else if (primaryfield_dc.ContainsKey(field))
                    {
                        _primaryfield_dc.Add(field, primaryfield_dc[field]);
                    }
                }
                primaryfield_dc = _primaryfield_dc;
            }
            if (primaryfield_dc == null || primaryfield_dc.Count() == 0)
            {
                throw new Exception("删除语句需要设置主键存在的条件才能执行！");
            }
            else
            {

                StringBuilder sqlbuilder_pre = new StringBuilder("update " + tableName + " set ");
                int counter = 0;
                foreach (string key in fields_dc.Keys)
                {
                    counter++;
                    if (withOutfield.Contains(key)) continue;
                    sqlbuilder_pre.AppendFormat("{0}=@{0}{1}", key, (counter == fields_dc.Count ? "  " : ","));

                    parameters.Add(new DbParam("@" + key, fields_dc[key]));
                }

                sqlbuilder_pre.Append(" where ");
                foreach (var pfiled in primaryfield_dc.Keys)
                {
                    sqlbuilder_pre.AppendFormat("{0}=@{0} and ", pfiled);
                    if (parameters.Count(p => p.ParameterName.Equals("@" + pfiled)) == 0) parameters.Add(new DbParam("@" + pfiled, primaryfield_dc[pfiled]));
                }
                sqlbuilder_pre.Append(" 1=1 ;");
                
                return sqlbuilder_pre.ToString();
            }
        }
        /// <summary>
        /// 创建删除语句
        /// </summary>
        private string DeleteSqlString(string tableName, IDictionary<string, object> fields_dc, IDictionary<string, object> primaryfield_dc, ref IList<DbParam> parameters, string[] watcher)
        {
            if (watcher != null && watcher.Length > 0)
            {
                primaryfield_dc.Clear();
                foreach (string field in watcher)
                {
                    if (fields_dc.ContainsKey(field))
                        primaryfield_dc.Add(field, fields_dc[field]);
                }
            }

            StringBuilder sqlbuilder_pre = new StringBuilder("delete " + tableName + " where ");
            if (primaryfield_dc != null && primaryfield_dc.Count() > 0)
            {
                foreach (var pfiled in primaryfield_dc.Keys)
                {
                    sqlbuilder_pre.AppendFormat("{0}=@{0} and ", pfiled);
                    parameters.Add(new DbParam("@" + pfiled, primaryfield_dc[pfiled]));
                }
                sqlbuilder_pre.Append(" 1=1 ;");
            }
            else
            {
                throw new Exception("删除语句需要设置主键存在的条件才能执行！");
            }
            return sqlbuilder_pre.ToString();
        }
        /// <summary>
        /// 创建更新或创建语句
        /// </summary>
        private string CreateOrUpdateSqlString(string tableName, IDictionary<string, object> fields_dc, IDictionary<string, object> primaryfields_dc, ref IList<DbParam> parameters, string[] watcher)
        {
            IList<DbParam> parameters1 = null;
            if (watcher != null && watcher.Length > 0)
            {
                IDictionary<string, object> _primaryfields_dc = new Dictionary<string, object>();
                parameters1 = new List<DbParam>();
                foreach (string field in watcher)
                {
                    if (fields_dc.ContainsKey(field))
                    {
                        _primaryfields_dc.Add(field, fields_dc[field]);
                    }
                    else if (primaryfields_dc.ContainsKey(field))
                    {
                        _primaryfields_dc.Add(field, primaryfields_dc[field]);
                    }
                }
                primaryfields_dc = _primaryfields_dc;
            }
            else
            {
                foreach (string field in primaryfields_dc.Keys)
                {
                    parameters1.Add(parameters.FirstOrDefault(p => p.ParameterName.Equals("@"+field)));
                }
            }
            
            StringBuilder sqlbuilder = new StringBuilder("select count(1) from " + tableName + " where ");
            foreach (var pfiled in primaryfields_dc.Keys)
            {
                sqlbuilder.AppendFormat("{0}=@{0} and ", pfiled);

                parameters1.Add(new DbParam("@" + pfiled, primaryfields_dc[pfiled]));


            }
            sqlbuilder.Append(" 1=1;");

            object exists = this.GetSingle(sqlbuilder.ToString(), parameters1.ToArray());
            if (exists.ToSafeInt()>0)
                return UpdateSqlString(tableName, fields_dc, primaryfields_dc,ref parameters, watcher);
            else
                return CreateSqlString(tableName, fields_dc, ref parameters);
        }
        #endregion

        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool OrmCreate<T>(T entity)
        {
            IList<DbParam> parameters = null;
            string sql = BuildSqlAndParams(entity, DBAction.Create, false, out parameters);
            return this.ExecuteSql(sql, parameters.ToArray()) > 0;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool OrmDelete<T>(T entity, params string[] wather)
        {
            IList<DbParam> parameters = null;
            string sql = BuildSqlAndParams(entity, DBAction.Delete, false, out parameters, wather);
            return this.ExecuteSql(sql, parameters.ToArray()) > 0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool OrmUpdate<T>(T entity, params string[] wather)
        {
            IList<DbParam> parameters = null;
            string sql = BuildSqlAndParams(entity, DBAction.Update, false, out parameters, wather);
            return this.ExecuteSql(sql, parameters.ToArray()) > 0;
        }
        /// <summary>
        /// 创建或更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="watcher">监事的累的数据，相同更新不同新增</param>
        /// <returns></returns>
        public bool OrmCreateOrUpdate<T>(T entity, params string[] watcher)
        {
            IList<DbParam> parameters = null;
            string sql = BuildSqlAndParams(entity, DBAction.CreateOrUpdate, false, out parameters, watcher);
            return this.ExecuteSql(sql, parameters.ToArray()) > 0;
        }


        public entity OrmGetEntity<entity>(Expression<Func<entity, object>> propertyNameLambda)
        {
            MemberExpression minfo = null;
            if (propertyNameLambda.Body is UnaryExpression)
            {
                minfo = (MemberExpression)((UnaryExpression)propertyNameLambda.Body).Operand;
            }
            else
            {
                minfo = (MemberExpression)propertyNameLambda.Body;
            }
            return default(entity);
        }
    }
}
