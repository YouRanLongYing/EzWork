using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Dtos.Entities.Nexus
{
    /// <summary>
    /// 用户部门关系
    /// </summary>
    [Serializable]
    public class FW_U_Ref_Department
    {
        /// <summary>
        /// 登陆编号,主键
        /// </summary>
        public int login_id { set; get; }
        /// <summary>
        /// 部门编号,主键
        /// </summary>
        public int dep_id { set; get; }
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
