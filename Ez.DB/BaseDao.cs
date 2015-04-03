using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.DBContract;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;

namespace Ez.DB
{
    /// <summary>
    /// 底层数据持久层
    /// </summary>
    public class BaseDao : DbTool, IBaseDao
    {   
        /// <summary>
        /// 构造器，出发数据库配置信息的生成
        /// <param name="scope">指定索引</param>
        /// <param name="pluginName">插件名</param>
        /// </summary>
        public BaseDao(string scope, string pluginName) : base(scope,pluginName) {
        
        
        }

        /// <summary>
        /// 获取查询的1行1列数据
        /// </summary>
        /// <param name="sqlString">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public object GetSingle(string sqlString, params DbParam[] parameters)
        {
            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))//added
                {
                    BuildSqlCommand(command, null, sqlString, parameters);
                    object obj = command.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
            }
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string sqlString, params DbParam[] parameters)
        {
            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))//added
                {
                    BuildSqlCommand(command, null, sqlString, parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringCollection">SQL语句的哈希表（key为sql语句，value是该语句的OleDbParameter[]）</param>
        public int ExecuteSqlTran(IList<SqlCollection> sqlStringCollection)
        {
            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))
                {
                    using (DbTransaction trans = connection.BeginTransaction())
                    {
                        int executednum = 0;
                        try
                        {
                            foreach (SqlCollection query in sqlStringCollection)
                            {
                                BuildSqlCommand(command, trans, query.SQlString, query.DbParams);
                                int val = command.ExecuteNonQuery();
                                if (val > 0) executednum++;
                            }
                            trans.Commit();
                        }
                        catch (Exception exp)
                        {
                            trans.Rollback();
                            executednum = 0;
                            throw exp;
                        }
                        return executednum;
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataReader，注意对象 DbDataReader的关闭
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DbDataReader</returns>
        public void ExecuteReader(ExecuteReaderCallBack callback, string sqlString, params DbParam[] parameters)
        {
            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))
                {
                    BuildSqlCommand(command, null, sqlString, parameters);
                    try
                    {
                        DbDataReader myReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                        callback(myReader);
                        myReader.Close();
                    }
                    catch (Exception exp)
                    {
                        throw new Exception("调用ExecuteReader时DbDataReader发生了异常", exp);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string sqlString, params DbParam[] parameters)
        {
            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))
                {
                    BuildSqlCommand(command, null, sqlString, parameters);

                    DataSet ds = new DataSet();
                    DbDataAdapter dataAdapter = this.NewDataAdapter;
                    dataAdapter.SelectCommand = command;

                    dataAdapter.SelectCommand.CommandTimeout = 1200;//////////////????

                    dataAdapter.Fill(ds, "ds");
                    command.Parameters.Clear();
                    return ds;
                }
            }
        }

        /// <summary>
        /// 注意对象 DbDataReader的关闭
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public void RunProcedureToDbReader(ExecuteReaderCallBack callback, string storedProcName, params DbParam[] parameters)
        {
            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))//added
                {
                    try
                    {
                        BuildProcCommand(command, storedProcName, parameters);
                        DbDataReader returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                        callback(returnReader);
                        returnReader.Close();
                    }
                    catch (Exception exp)
                    {
                        throw new Exception("调用DbDataReader时DbDataReader发生了异常", exp);
                    }
                }
            }
        }
        /// <summary>
        /// 执行存储过程，并返回数据集
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">参数</param>
        /// <returns>数据集</returns>
        public DataSet RunProcedure(string storedProcName, params DbParam[] parameters)
        {
            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))//added
                {
                    DataSet ds = new DataSet();
                    BuildProcCommand(command, storedProcName, parameters);
                    DbDataAdapter dataAdapter = this.NewDataAdapter;

                    dataAdapter.SelectCommand = command;
                    dataAdapter.SelectCommand.CommandTimeout = 1200;//////////////????

                    dataAdapter.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>行影响数</returns>
        public int RunProcedureToEffect(string storedProcName, params DbParam[] parameters)
        {
            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))//added
                {
                    BuildProcCommand(command, storedProcName, parameters);
                    int effect = command.ExecuteNonQuery();
                    foreach (DbParameter param in command.Parameters)
                    {
                        if (param.Direction != ParameterDirection.InputOutput && param.Direction != ParameterDirection.Output) continue;
                        DbParam dbparam = parameters.FirstOrDefault(p => p.Direction.Equals(ParameterDirection.Output) && (p.Direction.Equals(ParameterDirection.Output) || p.Direction.Equals(ParameterDirection.InputOutput)));
                        if (dbparam != null && dbparam.Value != DBNull.Value)
                        {
                            dbparam.Value = param.Value;
                        }
                    }
                    return effect;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>输出参数集合</returns>
        public Hashtable RunProcedureToOutPut(string storedProcName, params DbParam[] parameters)
        {

            using (DbConnection connection = this.NewConnection)
            {
                using (DbCommand command = this.NewCommand(connection))//added
                {
                    BuildProcCommand(command, storedProcName, parameters);
                    var effectrow = command.ExecuteNonQuery();
                    Hashtable output = new Hashtable();
                    if (effectrow > 0)
                    {
                        foreach (DbParameter param in command.Parameters)
                        {
                            if (param.Direction == ParameterDirection.Output && !output.ContainsKey(param.ParameterName))
                            {
                                output.Add(param.ParameterName, param.Value);
                            }
                        }
                    }
                    return output;
                }
            }
        }

        /// <summary>
        /// 构建参数列表
        /// </summary>
        private void BuildSqlCommand(DbCommand command, DbTransaction trans, string sqlString, params DbParam[] parameters)
        {
            if (command != null && command.Connection != null)
            {
                if (command.Connection.State != ConnectionState.Open) command.Connection.Open();
                if (trans != null) command.Transaction = trans;
                command.CommandType = CommandType.Text;
                command.CommandText = Syntax(sqlString, ref command, parameters);
            }
            else
            {
                throw new NullReferenceException("请设置command的实例和commond.Connection 实例");
            }
        }
        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private void BuildProcCommand(DbCommand command, string storedProcName, params DbParam[] parameters)
        {
            if (command != null && command.Connection != null)
            {
                if (command.Connection.State != ConnectionState.Open) command.Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Syntax(storedProcName, ref command, parameters);
            }
            else
            {
                throw new NullReferenceException("请设置command的实例和commond.Connection 实例");
            }
        }
    }
}
