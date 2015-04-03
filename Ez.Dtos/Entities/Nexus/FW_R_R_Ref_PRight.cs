using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Dtos.Entities.Nexus
{
    /// <summary>
    /// 页面级权限关系
    /// </summary>
    [Serializable]
    public class FW_R_R_Ref_PRight
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
        /// 权限编号
        /// </summary>
        public int right_id { set; get; }
        /// <summary>
        /// 子权限编号-页面级权限
        /// </summary>
        public int cright_id { set; get; }
        /// <summary>
        /// 是否开启此功能
        /// </summary>
        public int open_page_right { set; get; }
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
