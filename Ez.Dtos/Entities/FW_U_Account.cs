using System;
using Ez.Dtos.Library;
using Ez.Core.Attributes;

namespace Ez.Dtos.Entities
{
    [AsTable]
    [Serializable]
    public class FW_U_Account : BaseEntity 
    {
        /// <summary>
        /// 账户编号
        /// </summary>
        [AsField(Primary=true,Auto=true)]
        public int id { set; get; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string login_name { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { set; get; }
        /// <summary>
        /// 状态，0 默认正常
        /// </summary>
        public int status { set; get; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int login_num { set; get; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime last_loginTime { set; get; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime regist_time { set; get; }
        /// <summary>
        /// 注册IP
        /// </summary>
        public string regist_ip { set; get; }
        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string last_login_ip { set; get; }
        [IgnoreField]
        public string ts { set; get; }
    }
}
