using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.Buttons;
using Ez.XControls.Library;
using Ez.XControls.Properties;

namespace Ez.XControls.Menus
{
    /// <summary>
    /// 表示一个按钮项
    /// </summary>
    [DefaultEvent("Click")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Button))]
    public class XMenuItem : CtrlSolution
    {
        private InstanceTargetCtrl instancePage;
        public XMenuItem():this(null)
        {

        }
        public XMenuItem(InstanceTargetCtrl instancePage)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable, true);
            this.instancePage = instancePage;
        }


        [EditorBrowsable()]
        /// <summary>
        /// 目标所在容器
        /// </summary>
        public Panel TargetContainer { set; get; }

        /// <summary>
        /// 按钮调用的窗体全路径
        /// </summary>
        [EditorBrowsable()]
        public string Src { set; get; }
        private bool SrcHandleCreated = false;
        private Control targetCtrl;
        public Control TargetCtrl
        {
            get
            {
                if (targetCtrl == null || !SrcHandleCreated)
                {
                    if (!string.IsNullOrEmpty(this.Src) && instancePage!=null)
                    {
                        targetCtrl = instancePage(this.Src);
                        SrcHandleCreated = true;
                    }
                }
                return targetCtrl;
            }
            set
            {
                targetCtrl = value;
            }
        }

        #region 私有成员
        private XMenu child;
        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        protected bool pressDown = false;
        /// <summary>
        /// 鼠标是否进入
        /// </summary>
        protected bool mouseInMenu = false;
        /// <summary>
        /// 
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion

        #region 公开属性
        private Color mouseOverColor;
        /// <summary>
        /// 鼠标悬浮在按钮上时按钮的背景色
        /// </summary>
        [Category("外观"),
        Description("获取或设置鼠标悬浮在按钮上时按钮的背景色"),
        DefaultValue(typeof(Color), "120, 205, 230, 247")]
        public Color MouseOverColor
        {
            get
            {
                if (mouseOverColor == Color.Empty)
                {
                    mouseOverColor = Color.FromArgb(120, 205, 230, 247);
                    this.Invalidate();
                }
                return mouseOverColor;
            }
            set
            {
                mouseOverColor = value;
                this.Invalidate();
            }
        }

        private Color mousePressedColor;
        /// <summary>
        /// 鼠标在按钮按下时的背景色
        /// </summary>
        [Category("外观"),
        Description("获取或设置鼠标在按钮按下时的背景色"),
        DefaultValue(typeof(Color), "120, 146, 192, 224")]
        public Color MousePressedColor
        {
            get
            {
                if (mousePressedColor == Color.Empty)
                {
                    mousePressedColor = Color.FromArgb(120, 146, 192, 224);
                    this.Invalidate();
                }
                return mousePressedColor;
            }
            set
            {
                mousePressedColor = value;
                this.Invalidate();
            }
        }

        private Color mouseNormalColor;
        /// <summary>
        /// 鼠标的默认背景色
        /// </summary>
        [Category("外观"),
        Description("获取或设置鼠标的默认背景色"),
        DefaultValue(typeof(Color), "120, 255, 255, 255")]
        public Color MouseNormalColor
        {
            get
            {
                if (mouseNormalColor == Color.Empty)
                {
                    mouseNormalColor = Color.FromArgb(120, 255, 255, 255);
                    this.Invalidate();
                }
                return mouseNormalColor;
            }
            set
            {
                mouseNormalColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 按钮前面的图标，无图片时留空占位
        /// </summary>
        [Category("外观"),
        Description("按钮前面的图标，无图片时留空占位")]
        public Image Ico { set; get; }

        /// <summary>
        /// 获取或设置子级菜单
        /// </summary>
        public virtual XMenu Child
        {
            set
            {
                this.child = value;
                if (this.child != null)
                {
                    this.child.Parent = this;
                }
            }
            get
            {
                return this.child;
            }
        }
        #endregion

        #region 非公开属性
        /// <summary>
        /// 是否包含子菜单
        /// </summary>
        public virtual bool HasChild
        {
            get
            {
                return this.Child != null;
            }
        }
        #endregion

        #region 公开行为

        #endregion

        #region 内部行为
        /// <summary>
        /// 鼠标按钮
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            pressDown = true;
            this.Active(e);
            this.Invalidate();
        }
        /// <summary>
        /// 鼠标松开
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            pressDown = false;
            this.Invalidate();

        }
        /// <summary>
        /// 鼠标进入
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseInMenu = true;
            this.Invalidate();
            this.Active(e);

        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseInMenu = false;
            this.Invalidate();
        }
        /// <summary>
        /// 绘制
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Color color;
            if (this.mouseInMenu && !this.pressDown)
            {

                color = this.MouseOverColor;
            }
            else
            {
                color = this.pressDown ? this.MousePressedColor : this.MouseNormalColor;
            }

            Rectangle r = this.ClientRectangle;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new RectangleF(r.X, r.Y, r.Width--, r.Height--));
                e.Graphics.ResetClip();
                using (SolidBrush sb = new SolidBrush(color))
                {
                    e.Graphics.FillPath(sb, gp);
                }
            }
            var x = 25;// r.Width - textSize.Width - (this.HasChild ? 10 : 5);
            if (this.Ico != null)
            {
                e.Graphics.DrawImage(this.Ico, new Rectangle(5, (this.Height - this.Ico.Height) / 2, this.Ico.Width, this.Ico.Height));
                if (this.HasChild)
                {
                    e.Graphics.DrawImage(Resources.dpoint2, new Rectangle(this.Width - Resources.dpoint2.Width - 5, (this.Height - Resources.dpoint2.Height) / 2, Resources.dpoint2.Width, Resources.dpoint2.Height));
                }
            }
            SizeF textSize;
            if (!string.IsNullOrEmpty(this.Text))
            {
                textSize = e.Graphics.MeasureString(this.Text, this.Font);
                var y = (r.Height - textSize.Height) / 2f;


                e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), new PointF(x, y));
            }
        }
        #endregion

        #region 组件设计器生成的代码
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放由System.ComponentModel.Container释放的资源</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
        }
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器 
        /// 修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!this.HasChild && this.TargetCtrl != null)
            {
                if (!string.IsNullOrEmpty(this.TopLevelControl.Text))
                {
                    this.TopLevelControl.Text = this.TopLevelControl.Text.Split('-')[0] + "-" + this.Text;
                }
                else
                {
                    this.TopLevelControl.Text = "-" + this.Text;
                }
                foreach (Control item in this.TargetContainer.Controls)
                {
                    item.Dispose();
                }
                SrcHandleCreated = false;
                this.TargetContainer.Controls.Clear();
                if (this.TargetCtrl is Form)
                {
                    Form from = this.TargetCtrl as Form;
                    if (from.TopLevel)
                    {
                        from.TopLevel = false;
                    }
                    this.TargetCtrl = from;
                    this.TargetContainer.Controls.Add(this.TargetCtrl);
                    this.TargetCtrl.Show();
                }
                else if (this.TargetCtrl is WebBrowser)
                {
                    this.TargetContainer.BeginInvoke(new Func<int>(() =>
                    {
                        this.TargetContainer.Controls.Add(this.TargetCtrl);
                        this.TargetCtrl.Show();
                        return 0;
                    }));
                
                }
            }
#if !FMenu
            foreach (Control ctrl in this.TopLevelControl.Controls)
            {
                if (ctrl is XMenu)
                {
                    ctrl.Hide();
                }
            }
#endif
        }
        /// <summary>
        /// 动作激活
        /// </summary>
        /// <param name="e">参数</param>
        protected override void Active(EventArgs e)
        {
            XMenu xmenu = (this.Parent as XMenu);
            if (xmenu != null && xmenu.DisplayChildMenu != null)
            {
                xmenu.DisplayChildMenu.Hide();
            }
            if (this.HasChild)
            {
#if !FMenu
                this.TopLevelControl.Controls.Add(this.child);
                this.Child.BringToFront();
                this.Child.FireItem = this;
                if (xmenu != null)
                {
                    xmenu.DisplayChildMenu = this.Child;
                }
                this.Child.SetLocation(this);
                this.Child.Show();
                
#else
                if (this.Parent is XMenu)
                {
                    XMenu parent = this.Parent as XMenu;
                    parent.IsActived = false;
                    this.Child.IsActived = true;
                }
                this.Child.Show();
                this.Child.SetLocation(this);
#endif
               
            }
        }
    }
}
