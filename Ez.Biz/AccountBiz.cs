using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;
using Ez.Dtos;
using System.Data.Common;
using Ez.Dtos.Entities;
using Ez.UI.HtmlExtend;
using Ez.Helper;
using Ez.Core;
using Ez.Config;
using Ez.Dtos.Library;
using Ez.DBContract;
using Spring.Transaction.Interceptor;
using Ez.Cache;
using System.Data;
namespace Ez.Biz
{
    /// <summary>
    /// 系统账户数据管理业务
    /// </summary>
    public partial class AccountBiz : DefaultBiz, IAccountBiz
    {
        private void Test() {

            FW_U_Account entity = new FW_U_Account
            {
                login_name = "eeeeeee",
                password = "123456",
                regist_time = DateTime.Now,
                last_loginTime = DateTime.Now,
                last_login_ip = Tools.GetClientIp(),
                login_num = 0,
                regist_ip = Tools.GetClientIp(),
                status = 0
            };
            this.UcDb.OrmCreate<FW_U_Account>(entity);

            object objs = this.UcDb.GetSingle("select * from FW_U_Account where login_name='" + entity.login_name+ "'");

            entity.password = "wwwwwwwwwwwwwwwwwwwwwwwwwwww";

            this.UcDb.OrmUpdate<FW_U_Account>(entity,"login_name");

            this.UcDb.OrmCreateOrUpdate(new FW_U_Account
            {
                login_name = "OrmCreateOrUpdate2",
                password = "123477",
                regist_time = DateTime.Now,
                last_loginTime = DateTime.Now,
                last_login_ip = Tools.GetClientIp(),
                login_num = 0,
                regist_ip = Tools.GetClientIp(),
                status = 0
            }, "login_name", "password");

            this.UcDb.OrmCreateOrUpdate(new FW_U_Account
            {
                login_name = "OrmCreateOrUpdate2",
                password = "123477",
                regist_time = DateTime.Now,
                last_loginTime = DateTime.Now,
                last_login_ip = Tools.GetClientIp(),
                login_num = 100,
                regist_ip = Tools.GetClientIp(),
                status = 0
            }, "login_name", "password");

            this.UcDb.OrmDelete(entity, "login_name");
        
        }

        /// <summary>
        /// 登录请求处理
        /// </summary>
        /// <param name="dto">用户登录信息实体</param>
        /// <returns>登录结果</returns>
        public LoginInfoDto Login(LoginInfoDto dto)
        {
            //Test();
            object obj = this.UcDb.GetSingle("select id from FW_U_Account where login_name=@loginname and password=@pwd and status>=0 ",
                new DbParam("@loginname", dto.login_name),
                new DbParam("@pwd", Ez.Helper.CustomMD5.Powered(dto.password)));
            int loginid = obj.ToSafeInt();
            if (loginid > 0)
            {
                return CachePool<LoginInfoDto>(() =>
                {
                    dto = this.UcDb.GetEntity<LoginInfoDto>("select id,login_name,password,status,login_num,last_loginTime,regist_time,regist_ip,last_login_ip from FW_U_Account where login_name=@loginname and password=@pwd and status>=0",
                    new DbParam("@loginname", dto.login_name), new DbParam("@pwd", Ez.Helper.CustomMD5.Powered(dto.password)));
                    if (dto != null && dto.id > 0)
                    {
                        dto.Info = this.GetUserInfo(dto.id);
                        dto.Roles = this.GetRoles(dto.id);
                    }
                    return dto;
                }, CacheKeys.FRM_USER_LOGIN_INFO, loginid);
            }
            else
                return null;
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
                return this.UcDb.GetEntity<UserInfoDto>("select id,login_id,username,gender,age,id_card,telphone,mobile,qq,email,address,company,ext_data from FW_U_Info where login_id=@login_id", new DbParam("@login_id", login_id));
            }, CacheKeys.FRM_USER_INFORMATION, login_id);
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
                StringBuilder sql = new StringBuilder("update FW_U_Info set ");
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
                sql.Append("update_time=@update_time,");
                sql.Append("company=@company ");
                sql.Append(" where ");
                sql.Append("id = @id");
                if (this.UcDb.ExecuteSql(sql.ToString(),
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
                    new DbParam("@company", dto.company ?? ""),
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
                    RemoveCachePool(CacheKeys.FRM_USER_INFORMATION, dto.login_id);
                }
            }
            else
            {
                //新增
                if (this.UcDb.ExecuteSql("insert into FW_U_Info(login_id,username,gender,age,id_card,telphone,mobile,qq,email,address,create_time,company) values(@lid,@uname,@gender,@age,@idcard,@tel,@mobile,@qq,@email,@address,@create_time)",
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
                    new DbParam("@create_time", DateTime.Now), new DbParam("@company", dto.company)) > 0)
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
            if (this.UcDb.ExecuteSql("insert into FW_U_Account(login_name, [password], [status], login_num, regist_time, regist_ip,last_login_ip,last_logintime) values (@login_name, @password, @status, @login_num, @regist_time, @regist_ip, @last_login_ip , @last_logintime)",
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
        /// 获取登录信息
        /// </summary>
        /// <param name="login_id">登录id</param>
        /// <returns></returns>
        public LoginInfoDto GetLoginInfo(int login_id)
        {
            return this.UcDb.GetEntity<LoginInfoDto>("select id,login_name,password,login_num,last_loginTime,regist_time,regist_ip,last_login_ip from FW_U_Account where id=@id", new DbParam("@id", login_id));
        }

        /// <summary>
        /// 获取角色列表信息
        /// </summary>
        /// <param name="login_id">登录ID</param>
        /// <returns></returns>
        public IList<FW_U_Roles> GetRoles(int login_id)
        {
            IList<FW_U_Roles> roles = this.ProDb.GetEntities<FW_U_Roles>("select A.id, A.role_name,A.role_name_rs_key, A.state, A.flag, A.info, A.pro_id, A.creater, A.create_time from FW_U_Roles A left join FW_U_Ref_Roles B on A.id=B.roleid where B.login_id=@uid",
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
