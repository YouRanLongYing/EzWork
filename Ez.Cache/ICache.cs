using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Cache
{
    /// <summary>
    /// Cache 接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expire">过期时间（分钟）默认20分钟</param>
        void Set(string key, object value,int expire = 20);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">要移除的缓存的键</param>
        void Remove(string key);
    }
}
