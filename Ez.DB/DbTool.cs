namespace Ez.DB
{
    using System;
    using System.Data.Common;
    using System.Data;
    using Ez.DBContract;
    using Ez.Core.Interceptor;

    using OracleLib = System.Data.OracleClient;//Oracle.DataAccess.Client;//

    /// <summary>
    /// 数据库持久层对象生成工具
    /// </summary>
    public class DbTool
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string PluginName { set; get; }
        /// <summary>
        /// 数据库配置
        /// </summary>
        public ConnectionEntity ConnectionEntity { set; get; }
        /// <summary>
        /// 构造器，触发数据库配置信息的生成
        /// <param name="scope">指定索引</param>
        /// <param name="pluginName">插件名</param>
        /// </summary>
        public DbTool(string scope,string pluginName)
        {
            this.PluginName = pluginName;
            if (string.IsNullOrEmpty(this.PluginName))
            {
                ConnectionEntity = ConnectMaster.Get(scope);
            }
            else
            {
                ConnectionEntity = ConnectMaster.Get(this.PluginName,scope);
            }

            if (ConnectionEntity == null)
            {
                Log4NetManager.Output(new ExecuteInfo
                {
                    TargetType = this.GetType(),
                    Exception = new Exception("The connectionObj is not exits in current scopes! " + scope),
                    LogLevel = LogLevel.Error,
                    MethodName = "ConnectionEntity[" + scope + "]"
                });
            }
        }
        private string ReplaceParamName(string parameterName)
        {

            return parameterName;
        }
        /// <summary>
        /// SQL语法检测
        /// </summary>
        internal string Syntax(string sqlStringOrProcName, ref DbCommand cmd, params DbParam[] parameters)
        {
            if (cmd.Parameters != null) cmd.Parameters.Clear();
            if (parameters != null)
            {
                foreach (DbParam parameter in parameters)
                {
                    DbParameter param = null;
                    switch (ConnectionEntity.Dbtype)
                    {
                        case DBTypeEnum.Access:
                            {
                                param = new System.Data.OleDb.OleDbParameter(parameter.ParameterName.Replace("@", "?"), parameter.Value);
                            } break;
                        case DBTypeEnum.MSSQL:
                            {
                                param = new System.Data.SqlClient.SqlParameter(parameter.ParameterName.Replace("?", "@"), parameter.Value);
                            } break;
                        case DBTypeEnum.Oracle:
                            {
                                param = new OracleLib.OracleParameter(parameter.ParameterName.Replace("@", ":"), parameter.Value);
                            } break;
                        case DBTypeEnum.MySql:
                            {
                                param = new MySql.Data.MySqlClient.MySqlParameter(parameter.ParameterName.Replace("@", "?").Replace("[", "").Replace("]", ""), parameter.Value);
                            } break;
                    }
                    if (param != null)
                    {
                        param.Direction = parameter.Direction;
                        if (cmd.CommandType == CommandType.StoredProcedure
                            && (param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input)
                            && (param.Value == null))
                        {
                            param.Value = DBNull.Value;
                        }
                        else if (!parameter.ParameterName.Equals(param.ParameterName))
                        {
                            sqlStringOrProcName.Replace(parameter.ParameterName, param.ParameterName);
                        }
                        cmd.Parameters.Add(param);
                    }
                }
            }
            return sqlStringOrProcName;
        }

        /// <summary>
        /// 当前数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return ConnectionEntity.ConnectionString;
            }
        }
        /// <summary>
        /// 实例新连接
        /// </summary>
        public DbConnection NewConnection
        {
            get
            {
                DbConnection _conn = null;
                string _connectionString = this.ConnectionString;
                switch (ConnectionEntity.Dbtype)
                {
                    case DBTypeEnum.Access:
                        {
                            _conn = new System.Data.OleDb.OleDbConnection(_connectionString);
                        } break;
                    case DBTypeEnum.MSSQL:
                        {
                            _conn = new System.Data.SqlClient.SqlConnection(_connectionString);
                        } break;
                    case DBTypeEnum.Oracle:
                        {
                            _conn = new OracleLib.OracleConnection(_connectionString);
                        }
                        break;
                    case DBTypeEnum.MySql:
                        {
                            _conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString);
                        }
                        break;
                }
                if (_conn == null) throw new Exception("数据库连接类型不存在:" + ConnectionEntity.Dbtype.ToString());
                _conn.Open();
                return _conn;
            }
        }
        /// <summary>
        /// 实例新的数据适配
        /// </summary>
        public DbDataAdapter NewDataAdapter
        {
            get
            {
                DbDataAdapter _adp = null;
                switch (ConnectionEntity.Dbtype)
                {
                    case DBTypeEnum.Access:
                        {
                            _adp = new System.Data.OleDb.OleDbDataAdapter();
                        } break;
                    case DBTypeEnum.MSSQL:
                        {
                            _adp = new System.Data.SqlClient.SqlDataAdapter();
                        } break;
                    case DBTypeEnum.Oracle:
                        {
                            _adp = new OracleLib.OracleDataAdapter();
                        } break;
                    case DBTypeEnum.MySql:
                        {
                            _adp = new MySql.Data.MySqlClient.MySqlDataAdapter();
                        } break;
                }
                return _adp;
            }
        }
        /// <summary>
        ///  当前数据库命令对象
        /// </summary>
        protected DbCommand NewCommand(DbConnection connect)
        {
                DbCommand _cmd = null;
                switch (ConnectionEntity.Dbtype)
                {
                    case DBTypeEnum.Access:
                        {
                            _cmd = new System.Data.OleDb.OleDbCommand();
                        } break;
                    case DBTypeEnum.MSSQL:
                        {
                            _cmd = new System.Data.SqlClient.SqlCommand();
                        } break;
                    case DBTypeEnum.Oracle:
                        {
                            _cmd = new OracleLib.OracleCommand();
                        } break;
                    case DBTypeEnum.MySql:
                        {
                            _cmd = new MySql.Data.MySqlClient.MySqlCommand();
                        }
                        break;
                }
                if (_cmd != null && connect!=null)
                {
                    _cmd.Connection = connect;
                }
                else
                {
                    throw new Exception("Commond或Connection为空!");
                }
                return _cmd;
        }
    }
}

