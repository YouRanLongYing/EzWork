using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.Library;
using Ez.XControls.Menus;
using Ez.XControls.Properties;

namespace Ez.XControls.Buttons
{
    /// <summary>
    /// 按钮图片在按钮中的位置
    /// </summary>
    public enum ButtonImgPos
    {
        Top,
        Left,
        Right,
        Bottom,
        Normal
    }
    public delegate Control InstanceTargetCtrl(string uri);
    [DefaultEvent("Click")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Button))]
    public class XButton : CtrlSolution
    {
        private ToolTip tip = new ToolTip();
        private ButtonImgPos imgPos = ButtonImgPos.Top;
        /// <summary>
        /// 图片在按钮中的位置
        /// </summary>
        [Category("外观"),
        Description("获取或设置图片在按钮中的位置")]
        public ButtonImgPos ImgPos
        {
            get { return imgPos; }
            set { imgPos = value; }
        }
        private InstanceTargetCtrl instancePage;
        //是否作为菜单级别的按钮
        internal bool IsMenuBtn = false;
        #region 私有成员
        private const int EnterKeyCode = 13;
        private XMenu child;
        #endregion
        public XButton()
            : this(null)
        {

        }
        public XButton(InstanceTargetCtrl instancePage)
        {
            this.SetBgColorSolution(new CtrlBgColorInfo()
            {
                EnterBgColor = Color.FromArgb(120, 205, 230, 247),
                PressedBgColor = Color.FromArgb(120, 146, 192, 224),
                LeaveBgColor = Color.FromArgb(120, 255, 255, 255)
            });
            this.SetArrowSolution(new CtrlArrowInfo
            {
                HightLightArrow = Resources.dpoint2,
                NormalArrow = Resources.dpoint
            });
            this.instancePage = instancePage;
        }

        #region 公开属性
         [Category("外观"),
        Description("设置控件的tip")]
        public string TipMsg { set; get; }
        /// <summary>
        /// 按钮图标
        /// </summary>
        private Image image;
        [Category("外观"),
        Description("获取或设置按钮的图标")]
        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 控件文本
        /// </summary>
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [SettingsBindable(true)]
        public override string Text { get; set; }
        /// <summary>
        /// 按钮调用的窗体全路径
        /// </summary>
        [EditorBrowsable()]
        public string Src { set; get; }

        /// <summary>
        /// 此按钮是否为包含子菜单的父级菜单按钮
        /// </summary>
        [Browsable(false)]
        public bool IsMutil
        {
            get
            {
                return this.child != null;
            }
        }
        /// <summary>
        /// 目标所在容器
        /// </summary>
        public Panel TargetContainer { set; get; }
        private Control targetCtrl;
        private bool SrcHandleCreated = false;
        public Control TargetCtrl
        {
            get
            {
                if (targetCtrl == null || !SrcHandleCreated)
                {
                    if (!string.IsNullOrEmpty(this.Src) && instancePage != null)
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
        /// <summary>
        /// 子菜单项
        /// </summary>
        public XMenu Child
        {
            set
            {
                this.child = value;
                if (this.child != null && this.child.Parent == null)
                {
#if FMenu
                    this.child.Parent =this.child.TopParent =this;
#else
                    this.child.Parent = this;
#endif
                }
            }
            get
            {
                if (this.child != null)
                {
                    this.child.BringToFront();
                }
                return this.child;
            }
        }
        #endregion


        #region 内部行为
        /// <summary>
        /// 当前的背景色
        /// </summary>
        protected override Color CurrentBgColor
        {
            get
            {
                if (this.IsMousePressd && (IsMenuBtn && this.IsGotFocus))
                {
                    return this.BgColorInfo.PressedBgColor;
                }
                else if (this.IsMouseEnter && !this.IsMousePressd)
                {
                    return this.BgColorInfo.EnterBgColor;
                }
                else
                {
                    return this.BgColorInfo.LeaveBgColor;
                }

            }
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            MouseEventArgs args = ((MouseEventArgs)e);
            if (args.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Active(e);
            }
        }
        /// <summary>
        /// 重绘控件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle r = this.ClientRectangle;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new RectangleF(r.X, r.Y, r.Width--, r.Height--));
                e.Graphics.ResetClip();
                using (SolidBrush sb = new SolidBrush(this.CurrentBgColor))
                {
                    e.Graphics.FillPath(sb, gp);
                }
            }
            DraweButtonContent(e.Graphics, r);
        }
        /// <summary>
        /// 检测键盘回车
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == (char)EnterKeyCode)
            {
                Active(e);
            }
        }
        /// <summary>
        /// 鼠标松开事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!IsMenuBtn)
            {
                base.OnMouseUp(e);
            }
        }
        /// <summary>
        /// 动作激活
        /// </summary>
        /// <param name="e">参数</param>
        protected override void Active(EventArgs e)
        {
            if (this.IsMutil)
            {
#if !FMenu
                this.TopLevelControl.Controls.Add(this.child);
#endif

                this.Child.FireItem = this;
                this.Child.Show();
                this.Child.SetLocation(this);

            }
            else if (!this.IsMutil && this.TargetCtrl != null)
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

                if (this.TargetCtrl is Form)
                {
                    Form from = this.TargetCtrl as Form;
                    if (from.TopLevel)
                    {
                        from.TopLevel = false;
                    }
                    this.TargetCtrl = from;

                    this.TargetContainer.Controls.Clear();
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
        }
        bool settedTip = false;
        private void DraweButtonContent(Graphics g, Rectangle r)
        {
            PointF imagePoint = Point.Empty;
            PointF textPoint = PointF.Empty;
            Point arrowPoint = Point.Empty;
            bool hasText = !string.IsNullOrEmpty(this.Text);
            bool hasImg = this.Image != null;
            SizeF tSize = SizeF.Empty;
            if (hasText)
            {
                tSize = g.MeasureString(this.Text, this.Font);
            }
            switch (this.imgPos)
            {
                case ButtonImgPos.Top:
                    {
                        if (hasImg)
                        {
                            imagePoint = new PointF((r.Width - this.Image.Width) / 2f, 5f);
                            g.DrawImage(this.Image, new RectangleF(imagePoint, new SizeF(this.Image.Width, this.Image.Height)));
                        }
                        if (hasText && tSize != SizeF.Empty)
                        {
                            textPoint = new PointF((r.Width - tSize.Width + 2) / 2f, r.Height - 30);
                            g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textPoint);
                        }
                    } break;
                case ButtonImgPos.Normal:
                    {
                        if (hasImg)
                        {
                            imagePoint = new PointF((r.Width - this.Image.Width) / 2f, (r.Height - this.Image.Height) / 2f);
                            g.DrawImage(this.Image, new RectangleF(imagePoint, new SizeF(this.Image.Width, this.Image.Height)));
                        }
                        else if (hasText && tSize != SizeF.Empty)
                        {
                            textPoint = new PointF((r.Width - tSize.Width + 2) / 2f, (r.Height - tSize.Height + 2) / 2f);
                            g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textPoint);
                        }
                    };break;
            }

            if (this.IsMutil)
            {
                g.DrawImage(this.CurrentArrow,
                    new RectangleF(
                        (r.Width - this.CurrentArrow.Width) / 2f,
                        r.Height - this.CurrentArrow.Height - 5,
                        this.CurrentArrow.Width,
                        this.CurrentArrow.Height));
            }
            if (!settedTip&&!string.IsNullOrEmpty(this.TipMsg))
            {
                tip.SetToolTip(this, this.TipMsg);
                settedTip = true;
            }
        }
        #endregion
    }
}
