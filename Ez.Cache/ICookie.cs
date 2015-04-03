using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Cache
{
    /// <summary>
    /// Cookie接口
    /// </summary>
    public interface ICookie
    {
        /// <summary>
        /// Cookie对象索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string this[string key] { get;}
        /// <summary>
        /// Cookies赋值
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">键值</param>
        /// <param name="expiresday">有效天数</param>
        /// <param name="domain">当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com</param>
        /// <returns></returns>
        bool Set(string key, string value, int expiresday, string domain = "");
        /// <summary>
        /// Cookies赋值
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">键值</param>
        /// <param name="expires">过期时间</param>
        /// <param name="domain">当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com</param>
        /// <returns></returns>
        bool Set(string key, string value, DateTime expires, string domain = "");
        
        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        string Get(string key);
    
        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="domain">当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com</param>
        /// <returns></returns>
        bool Remove(string key, string domain = "");
   
    }
}
