using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Core.Framework.Attributes;

namespace UBIQ.Dtos.Framework.Models
{
    public class Frm_U_Roles
    {
        /// <summary>
        /// 编号
        /// </summary>
        [JsonItem]
        public int id { set; get; }
        /// <summary>
        /// 角色名
        /// </summary>
        [JsonItem]
        public string role_name { set; get; }
        /// <summary>
        /// 角色名在资源中的键
        /// </summary>
        public string role_name_rs_key { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        [JsonItem]
        public int state { set; get; }
        /// <summary>
        /// 标记
        /// </summary>
        public string flag { set; get; }
        /// <summary>
        /// 角色说明
        /// </summary>
         [JsonItem]
        public string info { set; get; }
        /// <summary>
        /// 产品注册号
        /// </summary>
        public int pro_id { set; get; }
        /// <summary>
        /// 语言
        /// </summary>
        public string lang { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
         [JsonItem]
        public int creater { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
         [JsonItem]
        public DateTime create_time { set; get; }
    }
}
