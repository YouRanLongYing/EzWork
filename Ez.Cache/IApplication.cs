using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Cache
{
    /// <summary>
    /// Application 接口
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// application对象索引
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        object this[string key]{set;get;}
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Set(string key, object value);
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        object Get(string key);
        /// <summary>
        /// 移除指定键的缓存
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);
        /// <summary>
        /// 移除全部缓存数据
        /// </summary>
        void RemoveAll();
    }
}
