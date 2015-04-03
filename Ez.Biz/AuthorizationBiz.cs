using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;

namespace Ez.Biz
{
    /// <summary>
    /// 系统授权业务
    /// </summary>
    public class AuthorizationBiz :DefaultBiz, IAuthorizationBiz
    {
        /// <summary>
        /// 获取角色的权限编号
        /// </summary>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="actionName">动作名</param>
        /// <returns></returns>
        public BizResult<int> LawfulRole(string controllerName,string actionName)
        {

            return new  BizResult<int>();

        }
        /// <summary>
        /// 获取子权限（页面级）
        /// </summary>
        /// <param name="role_id">角色id</param>
        /// <param name="right_id">模块权限编号</param>
        /// <returns></returns>
        public BizResult<bool> LawfulRight(int role_id, int right_id)
        {
            return new BizResult<bool>();

        }
    }
}
