using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UBIQ.Framework.XControls.Menus
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Panel))]
    //[Designer("System.Windows.Forms.Design.ParentControlDesigner,System.Design ")]
    public class XTabBar : UserControl
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public XTabBar()
        {
            InitializeComponent();
        }

        #region 委托与事件
        /// <summary>
        /// 当前菜单项发生更改时触发事件委托
        /// </summary>
        public delegate void ClickChanged(object sender, EventArgs e);
        /// <summary>
        /// 当tab发生单击且与上次不同时触发的事件
        /// </summary>
        public event ClickChanged ClickChangedEvent;
        #endregion

        #region 私有成员
        /// <summary>
        /// 菜单条Tab的边框颜色
        /// </summary>
        private Color borderColor = Color.FromArgb(225, 225, 225);
        /// <summary>
        /// 当前容器背景色
        /// </summary>
        private Color bgColor = Color.FromArgb(0, 114, 198);
        /// <summary>
        /// 各Tab的右间隔
        /// </summary>
        private int tabMarginR = 5;
        /// <summary>
        /// 首菜单
        /// </summary>
        private Label _header;
        #endregion

        #region 公开属性
        /// <summary>
        /// 当前被选中的菜单
        /// </summary>
        public Control CurrentMenu { set; get; }

        public string text;
        /// <summary>
        /// 文本
        /// </summary>
        [Category("外观"),
         Description("设置默认Tab的文本值"),
         DefaultValue("账户"),
        Browsable(true)]
        public override string Text { set { text = value; this.Invalidate(); } get { return text; } }
        #endregion

        #region 公开行为
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="ctrl">已添加的控件</param>
        public void AddTab(string tabName)
        {
            Label ctrl = new Label() { Text = tabName };
            ctrl.Paint += delegate(object sender, PaintEventArgs e)
            {
                var lbl = (Label)sender;

                if (CurrentMenu.Equals(lbl))
                {
                    ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle,
                    borderColor, 1, ButtonBorderStyle.Solid,
                    borderColor, 1, ButtonBorderStyle.Solid,
                    borderColor, 1, ButtonBorderStyle.Solid,
                    lbl.BackColor, 1, ButtonBorderStyle.Solid);
                }
                else
                {
                    ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle,
                    lbl.BackColor, 1, ButtonBorderStyle.Solid,
                    lbl.BackColor, 1, ButtonBorderStyle.Solid,
                    lbl.BackColor, 1, ButtonBorderStyle.Solid,
                    lbl.BackColor, 1, ButtonBorderStyle.Solid);
                }
            };
            ctrl.Click += delegate(object sender, EventArgs e)
            {
                Control _ctrl = sender as Control;
                if (!_ctrl.Equals(CurrentMenu) || CurrentMenu == null)
                {
                    CurrentMenu = _ctrl;
                    SetMenuStyle();
                    if (ClickChangedEvent != null)
                    {
                        ClickChangedEvent(sender, e);
                    }
                }

            };
            this.SuspendLayout();
            this.Controls.Add(ctrl);
            this.ResumeLayout();
        }
        #endregion

        #region 内部行为
        /// <summary>
        /// 初始化各控件
        /// </summary>
        private void InitializeComponent()
        {
            this._header = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _header
            // 
            this._header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this._header.ForeColor = System.Drawing.SystemColors.Control;
            this._header.Location = new System.Drawing.Point(0, 0);
            this._header.Name = "_header";
            this._header.Size = new System.Drawing.Size(64, 30);
            this._header.TabIndex = 0;
            this._header.Text = "header";
            this._header.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // XTabBar
            // 
            this.Controls.Add(this._header);
            this.Name = "XTabBar";
            this.Size = new System.Drawing.Size(790, 30);
            this.ResumeLayout(false);

        }
        /// <summary>
        /// 样式更改是的额外处理
        /// </summary>
        /// <param name="ctrl">当前控件</param>
        /// <param name="y">此控件套设置的y值</param>
        private delegate void FuncDelegate(Control ctrl, int y);
        /// <summary>
        /// 设置Tab按钮样式
        /// </summary>
        /// <param name="callback">回调，额外处理过程，主要是角色边框重绘</param>
        private void SetMenuStyle(FuncDelegate callback = null)
        {

            for (int i = 1; i < this.Controls.Count; i++)
            {
                Control ctrl = this.Controls[i];
                Label lbl = ctrl as Label;
                Graphics g = lbl.CreateGraphics();
                if (CurrentMenu == null)
                {
                    CurrentMenu = ctrl;
                }
                lbl.Invalidate();
                int y = CurrentMenu.Equals(lbl) ? 1 : 0;
                if (callback != null)
                {
                    callback(ctrl, y);
                }
                else
                {
                    lbl.Location = new Point(lbl.Location.X, y);
                }
                g.Dispose();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //设置控件边框，使用ControlPaint绘制边框，设置左上右边框无下边框1px实线淡灰色
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle,
            Color.Transparent, 0, ButtonBorderStyle.None,
            Color.Transparent, 0, ButtonBorderStyle.None,
            Color.Transparent, 0, ButtonBorderStyle.None,
            borderColor, 1, ButtonBorderStyle.Solid);
            //设置Tab按钮的文本
            this._header.Text = string.IsNullOrEmpty(Text) ? "header" : Text;
            //遍历当前容器中的控件
            if (this.Controls.Count > 0)
            {
                //设置菜单条固定Tab高度同父级容器高度一样
                this._header.Height = this.Height;
                //设置菜单条的固定Tab的背景色为当前容器的背景色
                this._header.BackColor = bgColor;
                //菜单条固定Tab的坐标为偏移（0,1）点
                this._header.Location.Offset(0, 1);
                //设置下一个紧挨着的Tab的X位置各Tab间隔5各单位
                int x = this._header.Width + tabMarginR;
                //设置Tab当前状态下的样式
                SetMenuStyle((Control ctrl, int y) =>
                {
                    //获取GDI+对象
                    Graphics gp = ctrl.CreateGraphics();
                    //控件必须为Label
                    Label lbl = ctrl as Label;
                    //可自由设置尺寸
                    lbl.AutoSize = false;
                    // 高度低容器一个单位
                    lbl.Height = this.Height - 1;
                    //文本垂直水平居中
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    //测量要设置的文本的尺寸
                    SizeF fontSize = gp.MeasureString(lbl.Text, lbl.Font);
                    if (fontSize.Width < 50)
                    {//tab的被显示为最小50各单位
                        lbl.Width = 50;
                    }
                    //设置当前处理的tab的位置
                    lbl.Location = new Point(x, y);
                    //设置下一个tab的X位置
                    x = x + lbl.Width + 5;
                });
            }
        }
        #endregion
    }
}
