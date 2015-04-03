using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using UBIQ.Core.Framework;
using System.Reflection;
using UBIQ.IBiz.Framework;
using UBIQ.Cache.Framework;

namespace UBIQ.Controllers.Framework.Lib
{
    [ErrorWatcher]
    [License]
    public class BaseController : Controller
    {
        private IShortUrlBiz _ShortUrlBizInstance;
        /// <summary>
        /// 用于Url短地址处理的业务对象
        /// </summary>
        public IShortUrlBiz ShortUrlBizInstance
        {
            get
            {
                if (this._ShortUrlBizInstance == null) this._ShortUrlBizInstance = Utils.GetSpringObject<IShortUrlBiz>("ShortUrlBiz");
                return this._ShortUrlBizInstance;
            }
        }

        ISession session;
        /// <summary>
        /// Session管理对象
        /// </summary>
        public ISession Session {
            get {
                if (session == null)
                {
                    session = Utils.GetSpringObject<ISession>("SessionTarget");
                }
                return session;
            }
        }
        ICookie cookie;
        /// <summary>
        /// Cookie管理对象
        /// </summary>
        public ICookie Cookie
        {
            get
            {
                if (cookie == null)
                {
                    cookie = Utils.GetSpringObject<ICookie>("CookieTarget");
                }
                return cookie;
            }
        }

        ICache cache;
        /// <summary>
        /// asp.net Cache管理对象
        /// </summary>
        public ICache Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = Utils.GetSpringObject<ICache>("CacheTarget");
                }
                return cache;
            }
        }

        IMemCached memCached;
        /// <summary>
        /// MemCached管理对象
        /// </summary>
        public IMemCached MemCached
        {
            get
            {
                if (memCached == null)
                {
                    memCached = Utils.GetSpringObject<IMemCached>("MemcachedTarget");
                }
                return memCached;
            }
        }

        IApplication application;
        /// <summary>
        /// Application管理对象
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (application == null)
                {
                    application = Utils.GetSpringObject<IApplication>("ApplicationTarget");
                }
                return application;
            }
        }
    }
}
