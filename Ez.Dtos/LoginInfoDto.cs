using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Dtos.Library;
using Ez.Dtos.Entities;
using Ez.UI;
using System.ComponentModel.DataAnnotations;
using Ez.UI;
using Ez.UI.Validations;
using System.Web.Mvc;

namespace Ez.Dtos
{
    /// <summary>
    /// 用户登录信息
    /// created by kongjing
    /// </summary>
    [Serializable]
    public class LoginInfoDto : DefaultDto
    {
        /// <summary>
        /// 账户编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 登录名
        /// </summary>
        [Required]
        [COM_N_LoginName]
        public string login_name { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [COM_N_Password]
        public string password { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [CDisplayName(LangKey = "COM_N_Password",StartWithConfirmName=true)]
        [CompareTo("password")]
        public string password2 { set; get; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int status { set; get; }
        /// <summary>
        /// 登录次数
        /// </summary>
        [COM_N_LoginNum]
        public int login_num { set; get; }
        /// <summary>
        /// 登录时间
        /// </summary>
        [COM_N_LastLoginTime]
        public DateTime last_loginTime { set; get; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [COM_N_RegTime]
        public DateTime regist_time { set; get; }
        /// <summary>
        /// 注册IP
        /// </summary>
        [COM_N_RegIP]
        public string regist_ip { set; get; }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        [COM_N_LastLoginIP]
        public string last_login_ip { set; get; }

        /// <summary>
        /// 用户角色一般只有一个角色
        /// </summary>
        public IList<FW_U_Roles> Roles { set; get; }

        /// <summary>
        /// 当前角色,后期单用户多角色的情况需要在这里处理
        /// </summary>
        public FW_U_Roles CurrentRole { get {

            return this.Roles.FirstOrDefault();
        
        } }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoDto Info { set; get; }
    }
}
