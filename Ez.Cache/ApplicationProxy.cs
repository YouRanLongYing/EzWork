using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Cache
{
    /// <summary>
    /// application 代理
    /// </summary>
    public class ApplicationProxy:IApplication
    {
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set(string key, object value)
        {
            System.Web.HttpContext.Current.Application[key] = value;
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public object Get(string key)
        {
           return System.Web.HttpContext.Current.Application[key];
        }
        /// <summary>
        /// 移除指定键的缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            System.Web.HttpContext.Current.Application.Remove(key);
        }
        /// <summary>
        /// 移除全部缓存数据
        /// </summary>
        public void RemoveAll()
        {
            System.Web.HttpContext.Current.Application.RemoveAll();
        }
        /// <summary>
        /// application对象索引
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }
    }
}
