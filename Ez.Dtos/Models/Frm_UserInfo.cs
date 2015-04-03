using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UBIQ.Dtos.Framework.Models
{
    public class Frm_UserInfo
    {
        /// <summary>
        /// 信息编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 账户编号
        /// </summary>
        public int login_id { set; get; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string username { set; get; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public int gender { set; get; }
        /// <summary>
        /// 用户年龄
        /// </summary>
        public int age { set; get; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string id_card { set; get; }
        /// <summary>
        /// 座机号码
        /// </summary>
        public string telphone { set; get; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { set; get; }
        /// <summary>
        /// 联系QQ
        /// </summary>
        public string qq { set; get; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string email { set; get; }
        /// <summary>
        /// 住址
        /// </summary>
        public string address { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { set; get; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime update_time { set; get; }
        /// <summary>
        /// 语种
        /// </summary>
        public string lang { set; get; }
    }
}
