using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Dtos;
using Ez.Dtos.Library;
using Ez.Dtos.Entities;
using System.ServiceModel;

namespace Ez.BizContract
{
    /// <summary>
    /// 框架用户业务处理模块
    /// </summary>
    [ServiceContract(Name = "AccountBizService", Namespace = "com.ubiq-soft")]
    public interface IAccountBiz : IDefaultBiz
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
        IList<FW_U_Roles> GetRoles(int login_id);
    }
}
