using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ez.UI.Entities
{
    /// <summary>
    /// 菜单模型
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// 窗口标题，非跨域情况下可不设置
        /// </summary>
        public string WinTitle { set; get; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 子菜单集合
        /// </summary>
        public IList<MenuGroup> Children { set; get; }
        /// <summary>
        /// 构成html标签后附加的html属性
        /// </summary>
        public object Attributes { set; get; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { set; get; }
        /// <summary>
        /// 菜单导向地址
        /// </summary>
        public string Url { set; get; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Ico { set; get; }
        /// <summary>
        /// 是否为窗口
        /// </summary>
        public bool IsWin { set; get; }
        /// <summary>
        /// 若为窗口，则窗口宽度
        /// </summary>
        public int Win_Width { set; get; }
        /// <summary>
        /// 若为窗口，则窗口高度
        /// </summary>
        public int Win_Height { set; get; }
        /// <summary>
        /// 窗口默认尺寸模式，若为Normal时 设置的 宽度和高度才可用
        /// </summary>
        public WinSizeEnum WinSize { set; get; }
        /// <summary>
        /// 是否使用左侧菜单列
        /// </summary>
        public bool UseLeftMenu { set; get; }
        /// <summary>
        /// 是否使用头部快捷搜索，需要对应的编码才能实现搜索功能
        /// </summary>
        public bool UseTopSearchBar { set; get; }
        /// <summary>
        /// 是否有权限,默认有
        /// </summary>
        public bool IsNoRight { set; get; }
    }
    /// <summary>
    /// 菜单所在组模型
    /// </summary>
    public class MenuGroup : IEnumerable
    {
        /// <summary>
        /// 菜单所在组编号
        /// </summary>
        public int GroupNum { set; get; }
        /// <summary>
        /// 菜单列表
        /// </summary>
        private IList<Menu> Menus { set; get; }
        public MenuGroup()
        {
            if (Menus == null) Menus = new List<Menu>();
        }
     
        public IEnumerator GetEnumerator()
        {
            foreach (var item in Menus)
            {
                yield return item;
            }
        }
        /// <summary>
        /// 为此组添加一个菜单
        /// </summary>
        /// <param name="menu"></param>
        public void Add(Menu menu)
        {
            this.Menus.Add(menu);
        }
    }
    /// <summary>
    /// 桌面顶部菜单模型
    /// </summary>
    public class TopMenu : Menu
    {
        ///// <summary>
        ///// 顶级菜单名
        ///// </summary>
        //public string Name { private set; get; }
        ///// <summary>
        /////  构成html标签后附加的html属性
        ///// </summary>
        //public object Attributes {private set; get; }
        /// <summary>
        /// 顶级菜单
        /// </summary>
        /// <param name="name">顶级菜单名</param>
        /// <param name="attributes">构成html标签后附加的html属性</param>
        public TopMenu(string name, object attributes = null)
        {
            this.Attributes = attributes;
            this.Name = name;
        }
        ///// <summary>
        ///// 子级菜单
        ///// </summary>
        //public IList<MenuGroup> Children { set; get; }
    }
    /// <summary>
    /// 桌面顶部菜单模型集合
    /// </summary>
    public class TopBar
    {
        /// <summary>
        /// 顶级菜单
        /// </summary>
        public IList<TopMenu> TopMenus { set; get; }

    }
}
