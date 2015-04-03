using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Dtos.Entities;
using Ez.Dtos.Library;

namespace Ez.BizContract
{
    /// <summary>
    /// 角色管理器接口
    /// </summary>
    public interface IRoleBiz:IDefaultBiz
    {
        /// <summary>
        /// 获取指定用户的权限
        /// </summary>
        /// <param name="login_id">登录用户编号</param>
        /// <param name="roleid">角色编号</param>
        /// <param name="webproj">是否为web项目，默认是</param>
        /// <returns></returns>
        BizResult<IList<FW_U_Rights>> GetRights(int login_id, int roleid, bool webproj = true);
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="dto">分页信息</param>
        /// <returns></returns>
        BizResult<PageDto<FW_U_Roles>> GetRoleList(PageDto<FW_U_Roles> dto);
    }
}
