using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.DBContract;
using Ez.Dtos;

namespace Ez.BizContract
{
    /// <summary>
    /// 业务模块默认基类
    /// </summary>
    public interface IDefaultBiz : IBaseBiz
    {
        /// <summary>
        /// 数据库操作管理器
        /// </summary>
        IDbMaster DbMaster {get; }
        /// <summary>
        /// 当前语言
        /// </summary>
        string CurrentLang { get; }
        /// <summary>
        /// 账户管理业务模块实例
        /// </summary>
        IAccountBiz AccountBiz { get; }
        /// <summary>
        /// 角色管理业务模块实例
        /// </summary>
        IRoleBiz RoleBiz { get; }
        /// <summary>
        /// 当前登录的用户
        /// </summary>
        LoginInfoDto CurrentUser { get; }
    }
}
