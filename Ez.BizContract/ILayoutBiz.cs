using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Dtos;

namespace Ez.BizContract
{
    /// <summary>
    /// 框架前端UI布局数据管理器
    /// </summary>
    public interface ILayoutBiz : IDefaultBiz
    {
        /// <summary>
        /// 获取可授权的功能模块，用于布局
        /// </summary>
        /// <param name="login_id">当前登录用户</param>
        /// <param name="roleid">角色编号</param>
        /// <returns></returns>
        BizResult<WindowDto> GetLayoutInfoWithUserRight(int login_id, int roleid);
        /// <summary>
        ///  获取可授权的功能模块，用于布局
        /// </summary>
        /// <param name="login_id">当前登录用户</param>
        /// <param name="roleid">角色编号</param>
        /// <returns></returns>
        BizResult<LayoutWinDto> GetLayoutDataWithUserRight(int login_id, int roleid);
    }
}
