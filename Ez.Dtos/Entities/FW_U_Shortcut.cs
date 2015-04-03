using System;
using Ez.Dtos.Library;

namespace Ez.Dtos.Entities
{
    [Serializable]
    public class FW_U_Shortcut : BaseEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 快捷键文本
        /// </summary>
        public string shortcut_name { get; set; }
       /// <summary>
        /// 快捷键文本资源键
        /// </summary>
        public string shortcut_name_rs_key { get; set; }
        /// <summary>
        /// 导向地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 语种
        /// </summary>
        public string lang { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 是否为窗口否则为普通连接
        /// </summary>
        public bool is_win { get; set; }
        /// <summary>
        /// 窗口打开时尺寸
        /// </summary>
        public int win_size { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string ico { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int sort { get; set; }
        /// <summary>
        /// 窗口宽
        /// </summary>
        public int win_width { get; set; }
        /// <summary>
        /// 窗口高
        /// </summary>
        public int win_height { get; set; }
    }
}
