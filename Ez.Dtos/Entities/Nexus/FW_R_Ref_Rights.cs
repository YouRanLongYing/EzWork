using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Dtos.Entities.Nexus
{
    /// <summary>
    /// 角色-权限关系
    /// </summary>
    [Serializable]
    public class FW_R_Ref_Rights
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public int role_id { set; get; }
        /// <summary>
        /// 权限
        /// </summary>
        public int right_id { set; get; }
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
