using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.BizContract
{
    public interface IAuthorizationBiz : IDefaultBiz
    {
        /// <summary>
        /// 获取角色的权限编号
        /// </summary>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="actionName">动作名</param>
        /// <returns></returns>
        BizResult<int> LawfulRole(string controllerName,string actionName);
        /// <summary>
        /// 获取子权限（页面级）
        /// </summary>
        /// <param name="role_id">权限ID</param>
        /// <param name="right_id"></param>
        /// <returns></returns>
        BizResult<bool> LawfulRight(int role_id, int right_id);
    }
}
