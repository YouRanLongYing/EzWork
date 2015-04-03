using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Core
{
    public class Constans
    {
        /// <summary>
        /// 框架设计人
        /// </summary>
        public const string DESIGNER = "kongjing";

        /// <summary>
        /// 框架设计人邮箱
        /// </summary>
        public const string DESIGNER_EMAIL = "endfalse@163.com,kongjing@ubiq-soft.com";

        /// <summary>
        /// 系统语言标识
        /// </summary>
        public const string SYS_LANAGUE_KEY ="LANAUAGEKEY";

        /// <summary>
        /// 系统UI配置文件的路径
        /// </summary>
        public const string SYS_UICONFIG_FILE_PATH = "~/Config/UI.config";

        /// <summary>
        /// 用于桌面打开窗口时需要记入的列表容器的id，对应桌面左侧的“窗口菜单”
        /// </summary>
        public const string OPENED_WINDOW_LIST_ID = "openedWins";
        /// <summary>
        /// 系统语言键，框架硬编码常量 对应数据库表Frm_U_Rights 列 dev_key的值"language" 共框架语言更换之用
        /// </summary>
        public const string DEVELOP_KEY_FOR_LANGUAGE = "language";
        /// <summary>
        /// 系统布局风格
        /// </summary>
        public const string SYS_LAYOUT_STYLE_KEY = "sys_style_layout";
    }
}
