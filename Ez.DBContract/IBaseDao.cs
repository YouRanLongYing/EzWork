using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;
using System.Data;

namespace Ez.DBContract
{
    /// <summary>
    /// 回调委托
    /// </summary>
    /// <param name="myReader"></param>
    public delegate void ExecuteReaderCallBack(DbDataReader myReader);
    /// <summary>
    /// 基础接口
    /// </summary>
    public interface IBaseDao
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        ConnectionEntity ConnectionEntity { set; get; }
        /// <summary>
        /// 获取查询的1行1列数据，带参数
        /// </summary>
        /// <param name="sqlString">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        object GetSingle(string sqlString, params DbParam[] parameters);
        /// <summary>
        /// 执行单条SQL
        /// </summary>
        /// <param name="sqlString">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回影响数</returns>
        int ExecuteSql(string sqlString, params DbParam[] parameters);
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="sqlStringCollection"></param>
        int ExecuteSqlTran(IList<SqlCollection> sqlStringCollection);
        /// <summary>
        /// 读取数据集
        /// </summary>
        /// <param name="callback">回调</param>
        /// <param name="sqlString">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        void ExecuteReader(ExecuteReaderCallBack callback, string sqlString, params DbParam[] parameters);
        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sqlString">SQL</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        DataSet Query(string sqlString, params DbParam[] parameters);
        /// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="callback">回调</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        void RunProcedureToDbReader(ExecuteReaderCallBack callback, string storedProcName, params DbParam[] parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DataSet</returns>
        DataSet RunProcedure(string storedProcName, params DbParam[] parameters);
        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>影响的行数</returns>
        int RunProcedureToEffect(string storedProcName, params DbParam[] parameters);
        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>输出参数值</returns>
        Hashtable RunProcedureToOutPut(string storedProcName, params DbParam[] parameters);
    }
}
