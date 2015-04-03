using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Dtos.Framework;
using UBIQ.Dtos.Framework.Lib;
using UBIQ.Dtos.Framework.Models;

namespace UBIQ.IBiz.Framework
{
    /// <summary>
    /// 框架用户物业处理中心
    /// </summary>
    public interface IUserManagerBiz : IDefaultBiz
    {
        /// <summary>
        /// 登录请求处理
        /// </summary>
        /// <param name="dto">用户登录信息实体</param>
        /// <returns>登录结果</returns>
        LoginInfoDto Login(LoginInfoDto dto);
        /// <summary>
        /// 用户基本信息
        /// </summary>
        /// <param name="login_id">登录ID（用户ID）</param>
        /// <returns>用户信息</returns>
        UserInfoDto GetUserInfo(int login_id);
        /// <summary>
        /// 添加或更新用户信息
        /// </summary>
        /// <param name="dto">用户信息实体</param>
        /// <returns></returns>
        BizResult<UserInfoDto> SetUserInfo(UserInfoDto dto);
        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="dto">用户登录信息实体</param>
        /// <returns></returns>
        BizResult<LoginInfoDto> CreateUser(LoginInfoDto dto);
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="dto">分页信息</param>
        /// <returns></returns>
        BizResult<PageDto<Frm_U_Roles>> GetRoleList(PageDto<Frm_U_Roles> dto);
        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="login_id">登录id</param>
        /// <returns></returns>
        LoginInfoDto GetLoginInfo(int login_id);
        /// <summary>
        /// 获取角色列表信息
        /// </summary>
        /// <param name="login_id">登录ID</param>
        /// <returns></returns>
        IList<Frm_U_Roles> GetRoles(int login_id);
    }
}
