using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.DBContract;
using Ez.Dtos;
using Ez.Cache;

namespace Ez.BizContract
{
    public delegate T function<T>();
    public interface IBaseBiz
    {
        ///// <summary>
        ///// 缓存管理，使用前请确保已经正确配置
        ///// </summary>
        //IMemCached MemCached { get; }

        /// <summary>
        /// 设置缓存并返回结果
        /// </summary>
        /// <typeparam name="T">要设置的类型</typeparam>
        /// <param name="func">委托的方法</param>
        /// <param name="key">缓存的键</param>
        /// <param name="args">如果key有格式化内容，作为格式需要的参数</param>
        /// <returns>缓存的数据</returns>
        T CachePool<T>(function<T> func, string key, params object[] args);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="args">参数</param>
        void RemoveCachePool(string key, params object[] args);
    }
}
