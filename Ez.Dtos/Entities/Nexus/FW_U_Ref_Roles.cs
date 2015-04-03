using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Dtos.Entities.Nexus
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    [Serializable]
    public class FW_U_Ref_Roles
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 账户编号
        /// </summary>
        public int login_id { set; get; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public int roleid { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int creater { set; get; }
    }
}
