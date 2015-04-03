using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.DBContract
{
    /// <summary>
    /// 数据库操作管理器
    /// </summary>
    public interface IDbMaster
    {
        /// <summary>
        /// 产品注册号
        /// </summary>
        int Proid {get; }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="scope">数据库连接标识</param>
        /// <returns></returns>
        IDefaultDao this[string scope] { get;}
        /// <summary>
        /// 获取对应类型的数据操作器
        /// </summary>
        /// <param name="scope">数据库连接标识</param>
        /// <returns>操作实例</returns>
        IDefaultDao Get(string scope);
        /// <summary>
        /// 获取默认的数据库操作实例
        /// </summary>
        /// <returns></returns>
        IDefaultDao FistOrDefault();
    }
}
