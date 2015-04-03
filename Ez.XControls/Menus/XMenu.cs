using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.Buttons;
using Ez.XControls.Library;

namespace Ez.XControls.Menus
{
    /// <summary>
    /// 子菜单控件,对多个按钮的封装
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Panel))]
    //[Designer("System.Windows.Forms.Design.ParentControlDesigner,System.Design ")]
    public class XMenu :
#if FMenu
 Form, ICtrlSolution
#else
        UserControl
#endif
    {
        public XMenu()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.Selectable, true);

#if FMenu
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
#endif
        }

        /// <summary>
        /// 当前显示子菜单
        /// </summary>
        public XMenu DisplayChildMenu { set; get; }

        /// <summary>
        /// 触发当前菜单显示的菜单项
        /// </summary>
        public CtrlSolution FireItem { set; get; }

        #region 私有成员
        private bool reLocation = true;
        private Control perTarget = null;
        private Color borderColor;
        #endregion

        #region 公开属性
        [Category("外观"),
        Description("设置边框的颜色"),
        DefaultValue(typeof(Color), "120,212,212,212")]
        public Color BorderColor
        {
            set
            {
                borderColor = value;
                this.Invalidate();
            }
            get
            {
                if (borderColor == Color.Empty)
                {
                    borderColor = Color.FromArgb(120, 212, 212, 212);
                }
                return borderColor;
            }
        }

        /// <summary>
        /// 设置此菜单依附父级容器的位置是否在底部否则就会右侧,默认在底部
        /// </summary>
        [
        Category("行为"),
        Description("设置此菜单依附父级容器的位置是否在底部否则就会右侧,默认在底部"),
        DefaultValue(true)
        ]
        public bool IsBottomPos { set; get; }
        #endregion

        #region 公开行为
        public void AddItem(XMenuItem item)
        {
            this.SuspendLayout();
            this.Controls.Add(item);
            this.ResumeLayout(false);
        }

        /// <summary>
        /// 设置子菜单的位置
        /// </summary>
        /// <param name="target">依附的目标控件</param>
        /// <param name="posBottom">是否在依附控件的下方出现否则就在右方出现，默认是</param>
        public void SetLocation(Control target)
        {
            //if ((this.reLocation || target != perTarget) && target != null)
            {
                Point p = GetAbsolutePoint(target);
                if (this.IsBottomPos)
                {
                    p.Y = p.Y + target.Height;
                }
                else
                {
                    //p.Y = p.Y + target.Height + this.Location.Y ;
                    p.X = p.X + target.Width;
                }
                this.Location = p;
                this.reLocation = false;
                this.perTarget = target;
            }
        }
        #endregion

        #region 内部行为
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 
            // XMenu
            // 
            
            this.Name = "XMenu";
#if FMenu
            this.ClientSize = new System.Drawing.Size(144, 32);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.XMenu_Activated);
            this.Deactivate += new System.EventHandler(this.XMenu_Deactivate);
#endif
            this.ResumeLayout(false);

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.SuspendLayout();
            if (this.Controls.Count > 0)
            {
                int y = 1, totalH = this.Controls[0].Height,addy=0;
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    Control ctrl = this.Controls[i];
                    if (ctrl.Text.Equals("-"))
                    {
                        ctrl.BackColor = Color.Silver;
                        ctrl.Height = 1;
                        addy += 1;
                        ctrl.Width = this.Width - 6;
                        ctrl.Location = new Point(3, y);
                    }
                    else
                    {
                        ctrl.Width = this.Width - 2;
                        ctrl.Location = new Point(1, y);
                    }
                    
                    if (i == this.Controls.Count - 1)
                    {
                        this.Height = totalH + 2 + addy;
                    }
                    else
                    {
                        y += ctrl.Height;
                        totalH += ctrl.Height;
                    }
                }
            }

            using (Pen pen = new Pen(this.BorderColor))
            {
                System.Drawing.Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
               
            }
            this.ResumeLayout(false);
        }
        private Point GetAbsolutePoint(Control target)
        {
            Point point = target.Location;
#if FMenu
            if (target.Parent != null)
#else
             if (target.Parent != null && target.Parent.Parent != null)
#endif

            {
                Point _point = GetAbsolutePoint(target.Parent);
                point.X = point.X + _point.X;
                point.Y = point.Y + _point.Y;
            }
            return point;
        }

        #endregion



#if FMenu
        public Control TopParent { set; get; }
        public new Control Parent { set; get; }
        public bool IsActived { set; get; }
        public void HideThis()
        {
            this.HideThis();
            this.IsActived = false;
            if (this.Parent != this.TopParent && this.Parent is XMenu)
            {
                (this.Parent as XMenu).IsActived = true;
            }
        }
        private void XMenu_Deactivate(object sender, EventArgs e)
        {
            if (this.IsActived)
            {
                HideParentAll(this);
                Console.WriteLine("1");
            }
            else
            {
                Console.WriteLine("0");
            }
        }
        private void HideParentAll(XMenu xmenu)
        {
            if (xmenu.Parent is XMenu)
            {
                HideParentAll(xmenu.Parent as XMenu);
            }
            xmenu.HideThis();
        }
        private void XMenu_Activated(object sender, EventArgs e)
        {
            
        }
#else
        public new CtrlSolution Parent { set; get; }
#endif
    }
}
