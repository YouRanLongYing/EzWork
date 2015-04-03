using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace Ez.DBContract
{
    public interface IDefaultDao : IBaseDao
    {
        IDefaultDao GetInstance(string scope);
        /// <summary>
        /// 获取最大序号
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        int GetMaxID(string tableName, string fieldName = "ID");
        /// <summary>
        /// 获取业务序号信息
        /// </summary>
        /// <param name="key">计数器的key</param>
        /// <returns></returns>
        FW_Squence GetSequenceEntity(string key);
        /// <summary>
        /// 获取业务序号的值
        /// </summary>
        /// <param name="key">计数器的key</param>
        /// <returns></returns>
        int GetSequenceVal(string key);
        /// <summary>
        /// 获取业务序号
        /// </summary>
        /// <param name="key">计数器的key,可以为表名，但必须保持唯一性</param>
        /// <returns></returns>
        string GetSequence(string key);
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="sqlString">执行的SQL</param>
        /// <param name="cmdParms">所需参数</param>
        /// <returns></returns>
        bool Exists(string sqlString, params DbParam[] cmdParms);
        /// <summary>
        /// 获取查询的实体信息
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sqlString">查询语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns></returns>
        T GetEntity<T>(string sqlString, params DbParam[] cmdParms) where T : class,new();
        /// <summary>
        /// 查询数据的实体集合,要求 查询结果的列名与T实例的属性名一致，可以通过 select login as loginName from ...形式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        IList<T> GetEntities<T>(string sqlString, params DbParam[] parameters) where T : class,new();
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询模型</param>
        /// <returns></returns>
        IList<T> QueryPaging<T>(QuerySql query, out int records, params DbParam[] parameters) where T : class,new();
        /// <summary>
        /// 获取单项数据结合，集合元素类型必须为基本数据类型
        /// </summary>
        /// <typeparam name="T">基本数据类型</typeparam>
        /// <param name="sqlString">查询语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        IList<T> GetList<T>(string sqlString, params DbParam[] parameters);
        /// <summary>
        /// 查询数据返回array列表
        /// </summary>
        /// <param name="sqlString">基本数据类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        IList<object> GetDataArray(string sqlString, params DbParam[] parameters);


        #region ORM
        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool OrmCreate<T>(T entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool OrmDelete<T>(T entity, params string[] wather);
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool OrmUpdate<T>(T entity,params string[] wather);
        /// <summary>
        /// 创建或更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="watcher">监事的累的数据，相同更新不同新增</param>
        /// <returns></returns>
        bool OrmCreateOrUpdate<T>(T entity,params string[] watcher);
        #endregion
    }
}
