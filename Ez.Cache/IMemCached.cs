using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ez.Cache
{
    /// <summary>
    /// Memcached 接口
    /// </summary>
    public interface IMemCached
    {
        /// <summary>
        /// 获取缓存数据的对象索引
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        object this[string key] { get; set; }

        #region Clear Func
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        void Clear(string key);
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="expiry">指定时间作为过期时间来清除缓存</param>
        void Clear(string key, DateTime expiry);
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="hashcode">指定缓存键的哈希值</param>
        /// <param name="expiry">指定时间作为过期时间来清除缓存</param>
        void Clear(string key, object hashcode, DateTime expiry);
        #endregion

        #region Get Func
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <returns>缓存数据</returns>
        Object Get(string key);
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="hashCode">指定缓存键的哈希值</param>
        /// <returns>缓存数据</returns>
        Object Get(string key, int hashCode);
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="hashCode">指定缓存键的哈希值</param>
        /// <param name="asstring">作为字符串类型返回</param>
        /// <returns>缓存数据</returns>
        Object Get(string key, int hashCode, bool asstring);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="value">缓存数据的值</param>
        void Set(string key, object value);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="value">缓存数据的值</param>
        /// <param name="expriy">设置缓存过期的时间</param>
        void Set(string key, object value, DateTime expriy);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="value">缓存数据的值</param>
        /// <param name="expriy">设置缓存过期的时间</param>
        /// <param name="hashCode">指定缓存键的哈希值</param>
        void Set(string key, object value, DateTime expriy, int hashCode);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <param name="value">缓存数据的值</param>
        /// <param name="hashCode">指定缓存键的哈希值</param>
        void Set(string key, object value, int hashCode);
        #endregion

        /// <summary>
        /// 清除所有缓冲区
        /// </summary>
        void FlushAll();

        /// <summary>
        /// 清除指定缓存服务的冲区
        /// </summary>
        [Obsolete("内部存在索引出界的bug，暂不允许使用")]
        void FlushAll(ArrayList servers);
        /// <summary>
        /// 检测缓存是否存在
        /// </summary>
        /// <param name="key">缓存数据的键</param>
        /// <returns>true:存在，false:不存在</returns>
        bool KeyExists(string key,params object[] args);
        /// <summary>
        /// 获取多个指定键的缓存数据
        /// </summary>
        /// <param name="keys">缓存键集合</param>
        /// <returns>键值对应的哈希表</returns>
        Hashtable GetMultiple(params string[] keys);
        /// <summary>
        /// 获取运行的信息
        /// </summary>
        /// <returns>键值对应的参数</returns>
        IDictionary<string, string> GetStatInfo();

        /// <summary>
        /// 获取缓存的代理对象
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns></returns>
        MemcachedProxy GetProxy(string poolName);
    }
}
