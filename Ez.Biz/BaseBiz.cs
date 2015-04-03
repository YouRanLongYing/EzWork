using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;
using Ez.Core;
using Ez.DBContract;
using System.Reflection;
using System.IO;
using Ez.Dtos.Entities;
using Ez.Dtos;
using Ez.Cache;
using System.Runtime.Serialization;

namespace Ez.Biz
{
    public class BaseBiz : IBaseBiz
    {
        IMemCached memCached;
        /// <summary>
        /// MemCached管理对象,请不要
        /// </summary>
        private IMemCached MemCached
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
        /// <summary>
        /// 快速设置缓存并返回结果
        /// </summary>
        /// <typeparam name="T">要设置的类型</typeparam>
        /// <param name="func">委托的方法</param>
        /// <param name="key">缓存的键</param>
        /// <param name="args">如果key有格式化内容，作为格式需要的参数</param>
        /// <returns>缓存的数据</returns>
        public T CachePool<T>(function<T> func, string key, params object[] args)
        {
            try
            {
                T t = default(T);
                if (MemcachedHandler.Config.opencache)
                {
                    t = (T)MemCached.Get(string.Format(key, args));
                    if (t == null)
                    {
                        t = func();
                        if (t != null)
                        {
                            MemCached.Set(string.Format(key, args), t);
                        }
                    }
                }
                else
                {
                    t = func();
                }
                return t;
            }
            catch (SerializationException sexp)
            {
                throw new Exception("请将缓存的类对象标记为可序列化[Serializable]！", sexp);
            }
            catch (Exception exp)
            {
                throw new Exception("获取缓存数据或执行查询时发生错误！", exp);
            }
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="args">参数</param>
        public void RemoveCachePool(string key, params object[] args)
        {
            MemCached.Clear(string.Format(key,args));
        }
    }
}
