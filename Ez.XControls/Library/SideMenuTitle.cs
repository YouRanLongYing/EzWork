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
    [ToolboxItem(false)]
    public class SideMenuTitle:XMenuItem
    {
        bool switchflag = false;
        private SideMenuChild child;

        public SideMenuTitle()
        {

            this.MouseNormalColor = this.MousePressedColor = Color.FromArgb(205, 205, 230, 247);
        }

        public new XSideMenuBar Parent { set; get; }
        /// <summary>
        /// 获取或设置子级菜单
        /// </summary>
        public new SideMenuChild Child
        {
            set
            {
                this.child = value;
            }
            get
            {
                return this.child;
            }
        }
        /// <summary>
        /// 是否包含子菜单
        /// </summary>
        public override bool HasChild
        {
            get
            {
                return this.Child != null;
            }
        }
        /// <summary>
        /// 鼠标按钮
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            pressDown = true;
            switchflag = !switchflag;
            if (!switchflag)
            {
                pressDown = false;
            }
            this.Active(e);
            this.Invalidate();
        }
        /// <summary>
        /// 鼠标松开
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            
        }
        /// <summary>
        /// 鼠标进入
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
           
           // mouseInMenu = true;
        }
        /// <summary>
        /// 动作激活
        /// </summary>
        /// <param name="e">参数</param>
        protected override void Active(EventArgs e)
        {
            if (this.HasChild)
            {
                if (switchflag)
                {
                    this.Child.FireItem = this;
                    this.Child.BringToFront();
                }
                if (this.Parent != null)
                {
                    this.Parent.DisplayViewMenus(this);
                }
            }
        }
    }
}
