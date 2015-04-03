using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Ez.Cache
{
    /// <summary>
    /// Cookie代理
    /// </summary>
    public class CookieProxy : ICookie
    {
        #region 单例实现
        private static CookieProxy instance = null;
        private CookieProxy()
        {

        }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static CookieProxy Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CookieProxy();
                }

                return instance;
            }
        }
        #endregion

        /// <summary>
        /// Cookie 索引
        /// </summary>
        /// <param name="key">cookie键</param>
        /// <returns></returns>
        public string this[string key]
        {
            get {
                return Get(key);
            }
        }
        /// <summary>
        /// Cookies赋值
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">键值</param>
        /// <param name="expiresday">有效天数</param>
        /// <param name="domain">当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com</param>
        /// <returns></returns>
        public bool Set(string key, string value, int expiresday, string domain = "")
        {
            return Set(key, value, DateTime.Now.AddDays(expiresday), domain);
        }
        /// <summary>
        /// Cookies赋值
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">键值</param>
        /// <param name="expires">过期时间</param>
        /// <param name="domain">当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com</param>
        /// <returns></returns>
        public bool Set(string key, string value, DateTime expires, string domain = "")
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(key);
                if (!string.IsNullOrEmpty(domain))
                {
                    Cookie.Domain = domain;
                }
                Cookie.Expires = expires;
                Cookie.Value = value;
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 读取Cookies
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public string Get(string key)
        {
            HttpCookie Cookie = System.Web.HttpContext.Current.Request.Cookies[key];
            if (Cookie != null)
            {
                return Cookie.Value.ToString();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="domain">当要跨域名访问的时候,给cookie指定域名即可,格式为.xxx.com</param>
        /// <returns></returns>
        public bool Remove(string key, string domain = "")
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(key);
                if (!string.IsNullOrEmpty(domain))
                {
                    Cookie.Domain = ".xxx.com";
                }
                Cookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
