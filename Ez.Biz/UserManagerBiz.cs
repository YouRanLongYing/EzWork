using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.IBiz.Framework;
using UBIQ.Dtos.Framework;
using System.Data.Common;
using UBIQ.Dtos.Framework.Models;
using UBIQ.UI.Framework.HtmlExtend;
using UBIQ.Helper.Framework;
using UBIQ.Core.Framework;
using UBIQ.Config.Framework;
using UBIQ.Dtos.Framework.Lib;
using UBIQ.IDataBase.Framework;
using Spring.Transaction.Interceptor;
using UBIQ.Cache.Framework;
namespace UBIQ.Biz.Framework
{
    public partial class UserManagerBiz : DefaultBiz, IUserManagerBiz
    {

        /// <summary>
        /// 登录请求处理
        /// </summary>
        /// <param name="dto">用户登录信息实体</param>
        /// <returns>登录结果</returns>
        public LoginInfoDto Login(LoginInfoDto dto)
        {
            object obj =  CacheProxy.Instance["LoginInfoDto"];
            if (obj == null)
            {
                dto = this.UCenterDb.GetEntity<LoginInfoDto>("select id,login_name,password,status,login_num,last_loginTime,regist_time,regist_ip,last_login_ip from Frm_UserAccount where login_name=@loginname and password=@pwd and status>=0",
                   new DbParam("@loginname", dto.login_name),new DbParam("@pwd", UBIQ.Helper.Framework.CustomMD5.Powered(dto.password)));
                if (dto != null && dto.id > 0)
                {
                    dto.Info = this.GetUserInfo(dto.id);
                    dto.Roles = this.GetRoles(dto.id);
                }
                CacheProxy.Instance["LoginInfoDto"] = dto;
            }
            else
            {
                dto = obj as LoginInfoDto;
            }
            return dto;
        }

        /// <summary>
        /// 用户基本信息
        /// </summary>
        /// <param name="login_id">登录ID（用户ID）</param>
        /// <returns>用户信息</returns>
        public UserInfoDto GetUserInfo(int login_id)
        {
            return CachePool<UserInfoDto>(() =>
            {
                return this.UCenterDb.GetEntity<UserInfoDto>("select id,login_id,username,gender,age,id_card,telphone,mobile,qq,email,address,ext_data from Frm_UserInfo where login_id=@login_id",new DbParam("@login_id", login_id));
            },CacheKeys.FRM_USER_INFORMATION, login_id);
        }

        /// <summary>
        /// 添加或更新用户信息
        /// </summary>
        /// <param name="dto">用户信息实体</param>
        /// <returns></returns>
        public BizResult<UserInfoDto> SetUserInfo(UserInfoDto dto)
        {
            BizResult<UserInfoDto> result;

            if (dto.id > 0)
            {
                //修改
                StringBuilder sql = new StringBuilder("update Frm_UserInfo set ");
                sql.Append("login_id=@login_id,");
                sql.Append("username=@username,");
                sql.Append("gender=@gender,");
                sql.Append("age=@age,");
                sql.Append("id_card=@id_card,");
                sql.Append("telphone=@telphone,");
                sql.Append("mobile=@mobile,");
                sql.Append("qq=@qq,");
                sql.Append("email=@email,");
                sql.Append("address=@address,");
                sql.Append("update_time=@update_time");
                sql.Append(" where ");
                sql.Append("id = @id");
                if (this.UCenterDb.ExecuteSql(sql.ToString(), 
                    new DbParam("@login_id", dto.login_id),
                    new DbParam("@username", dto.username),
                    new DbParam("@gender", dto.gender),
                    new DbParam("@age", dto.age),
                    new DbParam("@id_card", dto.id_card),
                    new DbParam("@telphone", dto.telphone),
                    new DbParam("@mobile", dto.mobile),
                    new DbParam("@qq", dto.qq),
                    new DbParam("@email", dto.email),
                    new DbParam("@address", dto.address),
                    new DbParam("@update_time", DateTime.Now),
                    new DbParam("@id", dto.id)) > 0)
                {
                    result = new BizResult<UserInfoDto>(true, dto);
                }
                else
                {
                    result = new BizResult<UserInfoDto>(false);
                }
                if (result != null)
                {
                    MemCached.Clear(string.Format(CacheKeys.FRM_USER_INFORMATION, dto.login_id));
                }
            }
            else
            {
                //新增
                if (this.UCenterDb.ExecuteSql("insert into Frm_UserInfo(login_id,username,gender,age,id_card,telphone,mobile,qq,email,address,create_time) values(@lid,@uname,@gender,@age,@idcard,@tel,@mobile,@qq,@email,@address,@create_time)", 
                    new DbParam("@lid", dto.login_id),
                    new DbParam("@uname", dto.username),
                    new DbParam("@gender", dto.gender),
                    new DbParam("@age", dto.age),
                    new DbParam("@idcard", dto.id_card),
                    new DbParam("@tel", dto.telphone),
                    new DbParam("@mobile", dto.mobile),
                    new DbParam("@qq", dto.qq),
                    new DbParam("@email", dto.email),
                    new DbParam("@address", dto.address),
                    new DbParam("@create_time", DateTime.Now)) > 0)
                {
                    result = new BizResult<UserInfoDto>(true, dto);
                }
                else
                {
                    result = new BizResult<UserInfoDto>(false);
                }
            }
            return result;
        }

        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="dto">用户登录信息实体</param>
        /// <returns></returns>
        public BizResult<LoginInfoDto> CreateUser(LoginInfoDto dto)
        {
            BizResult<LoginInfoDto> result;
            if (this.UCenterDb.ExecuteSql("insert into Frm_UserAccount(login_name, [password], [status], login_num, regist_time, regist_ip,last_login_ip,last_logintime) values (@login_name, @password, @status, @login_num, @regist_time, @regist_ip, @last_login_ip , @last_logintime)", 
                new DbParam("@login_name", dto.login_name),
                     new DbParam("@password", dto.password),
                     new DbParam("@status", 1),
                     new DbParam("@login_num", 0),
                     new DbParam("@last_logintime", DateTime.Now),
                     new DbParam("@regist_time", DateTime.Now),
                     new DbParam("@regist_ip", Tools.GetClientIp()),
                     new DbParam("@last_login_ip", Tools.GetClientIp())) > 0)
            {
                dto = this.Login(dto);
                result = new BizResult<LoginInfoDto>(true, dto);
            }
            else
            {
                result = new BizResult<LoginInfoDto>(false);
            }
            return result;
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="dto">传输模型</param>
        /// <returns>分页结果</returns>
        [Transaction]
        public BizResult<PageDto<Frm_U_Roles>> GetRoleList(PageDto<Frm_U_Roles> dto)
        {
            int records=0;
            dto.Results = this.UCenterDb.QueryPaging<Frm_U_Roles>(
            new QuerySql
            {
                SelectColumns = "id, role_name, role_name_rs_key,[state],info,creater,create_time",
                FromTableNames = "Frm_U_Roles",
                SequenceColumnName = "id",
                IsDesc = dto.IsDesc,
                PageIndex = dto.PageIndex,
                PageSize = dto.PageSize,
                OrderBy = dto.OrderBy
            }, out records);
            dto.Records = records;

            return new BizResult<PageDto<Frm_U_Roles>>(true, dto);
        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <param name="login_id">登录id</param>
        /// <returns></returns>
        public LoginInfoDto GetLoginInfo(int login_id)
        {
            return this.UCenterDb.GetEntity<LoginInfoDto>("select id,login_name,password,login_num,last_loginTime,regist_time,regist_ip,last_login_ip from Frm_UserAccount where id=@id",new DbParam("@id", login_id));
        }

        /// <summary>
        /// 获取角色列表信息
        /// </summary>
        /// <param name="login_id">登录ID</param>
        /// <returns></returns>
        public IList<Frm_U_Roles> GetRoles(int login_id)
        {
            IList<Frm_U_Roles> roles = this.UCenterDb.GetEntities<Frm_U_Roles>("select A.id, A.role_name,A.role_name_rs_key, A.[state], A.flag, A.info, A.pro_id, A.creater, A.create_time from Frm_U_Roles A left join Frm_U_Ref_Roles B on A.id=B.roleid where B.[uid]=@uid",
                new DbParam("@uid", login_id));
            roles.Where((a, b) =>
            {
                if (string.IsNullOrEmpty(a.role_name))
                    a.role_name = Tools.GetResourceString(a.role_name_rs_key);
                return false;
            });
            return roles;
        }
    }
}
