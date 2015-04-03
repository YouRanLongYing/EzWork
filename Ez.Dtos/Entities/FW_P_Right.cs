using System;
using Ez.Dtos.Library;

namespace Ez.Dtos.Entities
{
    [Serializable]
    public class FW_P_Right : BaseEntity
    {
        /// <summary>
        /// 权限编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 模块权限编号
        /// </summary>
        public int right_id { get; set; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string text_name { get; set; }
        /// <summary>
        /// 显示文本在资源中的键
        /// </summary>
        public string text_name_rs_key { get; set; }
        /// <summary>
        /// 执行动作类型
        /// </summary>
        public int action_type { get; set; }
        /// <summary>
        /// UI样式类型
        /// </summary>
        public int action_style { get; set; }
        /// <summary>
        /// 样式
        /// </summary>
        public string text_css { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 产品注册编号
        /// </summary>
        public int pro_id { get; set; }
    }
}
