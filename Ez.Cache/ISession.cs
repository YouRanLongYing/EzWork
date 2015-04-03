using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Cache
{
    /// <summary>
    /// Session 接口
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// 移除所有键值对
        /// </summary>
        void Clear();
        /// <summary>
        /// 获取或设置Session数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; }
        /// <summary>
        /// 添加缓存信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Set(string key, object value);
        /// <summary>
        /// 取消当前会话
        /// </summary>
        void Abandon();
        /// <summary>
        /// 清除掉所有非共同Session变量
        /// </summary>
        void ClearVarSessionAll();
    }
}
