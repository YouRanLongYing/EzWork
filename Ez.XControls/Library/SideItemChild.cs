using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.Menus;

namespace Ez.XControls.Library
{
    /// <summary>
    /// 菜单列表
    /// </summary>
    [ToolboxItem(false)]
    public class SideItemChild : XMenu
    {

        public SideItemChild()
        {
#if FMenu
            this.TopLevel = false;
#endif
        }
        /// <summary>
        /// 显示时的坐标点
        /// </summary>
        public Point ViewPoint { set; get; }
        public new XSideBar Parent { set; get; }
        public new void Hide()
        {
            base.Hide();
        }
        public new void Show() {

            base.Show();
        }
    }
}
