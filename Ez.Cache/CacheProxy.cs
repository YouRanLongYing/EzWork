using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCache = System.Web.Caching;
using System.Web;
namespace Ez.Cache
{
    /// <summary>
    /// asp.net 缓存代理
    /// </summary>
    public class CacheProxy : ICache
    {
        #region 单例实现
        private static CacheProxy instance = null;

        private CacheProxy()
        {

        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static CacheProxy Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CacheProxy();
                }
                return instance;
            }
        }
        #endregion


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Cache.Get(key);
                }
                return null;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
                }
            }
        }
        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expire">过期时间（分钟）默认20分钟</param>
        public void Set(string key, object value,int expire=20)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(expire));
            }
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">要移除的缓存的键</param>
        public void Remove(string key)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Remove(key);
            }
        }
    }
}
