using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UBIQ.Framework.XControls.Menus;

namespace UBIQ.Framework.XControls.Library
{
    /// <summary>
    /// 菜单列表
    /// </summary>
    [ToolboxItem(false)]
    public class SideMenuChild : XMenu
    {

        public SideMenuChild()
        {
            //临时
            this.TopLevel = false;
            this.Focus();
            this.Activate();
            this.TopMost = true;
        }
        /// <summary>
        /// 显示时的坐标点
        /// </summary>
        public Point ViewPoint { set; get; }
        public new XSideMenuBar Parent { set; get; }
        public new void Hide()
        {
            base.Hide();
        }
        public new void Show() {

            base.Show();
        }
    }
}
