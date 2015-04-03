using System;
using Ez.Dtos.Library;

namespace Ez.Dtos.Entities
{
    /// <summary>
    /// 用户权限表
    /// </summary>
    [Serializable]
    public class FW_U_Rights : BaseEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 权限名/菜单名
        /// </summary>
        public string right_name { set; get; }
        /// <summary>
        /// 权限名/菜单名在资源中的键
        /// </summary>
        public string right_name_rs_key { set; get; }
        /// <summary>
        /// 标题名
        /// </summary>
        public string title { set; get; }
        /// <summary>
        /// 标题名在资源中的键
        /// </summary>
        public string title_rs_key { set; get; }
        /// <summary>
        /// 受限制地址
        /// </summary>
        public string limit_url { set; get; }
        /// <summary>
        /// 为菜单是此菜单的父级菜单
        /// </summary>
        public int parent_id { set; get; }
        /// <summary>
        /// 是否收权限控制
        /// </summary>
        public bool un_ctrl { set; get; }
        /// <summary>
        /// 是否在无权限是显示
        /// </summary>
        [Obsolete("已丢弃使用的字段")]
        public bool is_noright_show { set; get; }
        /// <summary>
        /// 排序
        /// </summary>
        public int sort { set; get; }
        /// <summary>
        /// 是否为菜单
        /// </summary>
        public bool is_menu { set; get; }
        /// <summary>
        /// 为菜单时的分组
        /// </summary>
        public int group_num { set; get; }
        /// <summary>
        /// 是否为快捷方式
        /// </summary>
        public bool is_shortcut { set; get; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public int pro_id { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { set; get; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        public int creater { set; get; }
        ///// <summary>
        ///// 语种
        ///// </summary>
        //public string lang { set; get; }
        /// <summary>
        /// 是否为窗口否则为一个普通连接
        /// </summary>
        public bool is_win { set; get; }
        /// <summary>
        /// 窗口尺寸
        /// </summary>
        public int win_size{ set; get; }
        /// <summary>
        /// 图标，快捷按钮是使用
        /// </summary>
        public string ico { set; get; }
        /// <summary>
        /// 样式名，若为菜单是那么可以为其设置样式以定义一个小图标
        /// </summary>
        public string class_name { set; get; }
        /// <summary>
        /// 窗口宽度
        /// </summary>
        public int win_width { set; get; }
        /// <summary>
        /// 窗口高度
        /// </summary>
        public int win_height { set; get; }
        /// <summary>
        /// 用于查找指定模块，一般在产品中以硬编码的形式新增模块菜单
        /// </summary>
        public string dev_key { set; get; }

        /// <summary>
        /// 是否为Web项目
        /// </summary>
        public bool is_web_project { set; get; }
    }
}
