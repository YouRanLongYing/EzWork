using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace Ez.Cache
{
    /// <summary>
    /// Session代理
    /// </summary>
    public class SessionProxy:ISession
    {
        #region 单例实现
        private static SessionProxy instance = null;

        private SessionProxy()
        {

        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static SessionProxy Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SessionProxy();
                }

                return instance;
            }
        }
        #endregion

        #region 访问入口
        /// <summary>
        /// 获取或设置Session数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Session[key];
                }
                else
                    return null;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session[key] = value;
                }
            }
        }

        public void Set(string key, object value)
        {
            HttpContext.Current.Session.Add(key, value);
        }

        public void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        public void Abandon()
        {
            HttpContext.Current.Session.Abandon();
        }


        /// <summary>
        /// 清除掉所有非共通Session变量
        /// </summary>
        public void ClearVarSessionAll()
        {
            Type keysType = typeof(SessionKeys);
            FieldInfo[] fildInfos = keysType.GetFields(BindingFlags.Public|BindingFlags.GetField);
            if (fildInfos.Length > 0)
            {
                object keyObj = null;
                string key = null;
                foreach (var item in fildInfos)
                {
                    keyObj = item.GetValue(null);
                    key = keyObj == null ? null : keyObj.ToString();
                    if (key != null && key.StartsWith("FRM_"))
                    {
                        HttpContext.Current.Session.Remove(keyObj.ToString());
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 设置登录信息
        /// </summary>
        public void SetLoginInfo(object obj)
        {
            this.Set(SessionKeys.FRM_LOGININFO, obj);
        }
        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        public object GetLoginInfo()
        {
           return this[SessionKeys.FRM_LOGININFO];
        }

    }
}
