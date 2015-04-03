using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.Buttons;

namespace Ez.XControls.Menus
{
    /// <summary>
    /// 按钮分组容器
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Panel))]
    [Designer("System.Windows.Forms.Design.ParentControlDesigner,System.Design ")]
    public class XMenuGroup : UserControl
    {
        #region 私有成员
        private Label lbl_gpname;
        private Panel pnl_gp_wrapper;
        /// <summary>
        /// 
        /// </summary>
        private System.ComponentModel.Container components = null;
        #endregion

        #region 公开属性
        [Category("外观"),
         Description("设置分组控件要显示的名称")]
        public string GroupName { get { return this.lbl_gpname.Text; } set { this.lbl_gpname.Text = value; } }

        public Control.ControlCollection Chridren { get { return this.pnl_gp_wrapper.Controls; } }
        #endregion

        #region 公开行为
        public XMenuGroup()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            InitializeComponent();
        }
        public void AddControl(Control ctrl)
        {
            this.SuspendLayout();
            if (ctrl is XButton)
            {
                XButton btn = (ctrl as XButton);
                btn.IsMenuBtn = true;
                ctrl = btn;
            }
            ctrl.Parent = this;
            int ctrlnum = this.Chridren.Count;
            int rlen = 5;
            int X = 0;
            foreach (Control item in this.Chridren)
            {
                X += item.Width;
            }
            ctrl.Left = X + rlen * (ctrlnum + 1);

            this.Width = ctrl.Left + ctrl.Width + rlen;
            this.Chridren.Add(ctrl);
            this.ResumeLayout();
        }
        #endregion

        #region 内部行为
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Color color = Color.FromArgb(225, 225, 225);
            ControlPaint.DrawBorder(this.CreateGraphics(), this.ClientRectangle,
               Color.Transparent, 0, ButtonBorderStyle.None,
               Color.Transparent, 0, ButtonBorderStyle.None,
               Color.FromArgb(225, 225, 225), 1, ButtonBorderStyle.Solid,
               Color.Transparent, 0, ButtonBorderStyle.None);
        }
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
        private void InitializeComponent()
        {
            this.lbl_gpname = new System.Windows.Forms.Label();
            this.pnl_gp_wrapper = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lbl_gpname
            // 
            this.lbl_gpname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_gpname.Location = new System.Drawing.Point(3, 73);
            this.lbl_gpname.Name = "lbl_gpname";
            this.lbl_gpname.Size = new System.Drawing.Size(174, 14);
            this.lbl_gpname.TabIndex = 0;
            this.lbl_gpname.Text = "grpName";
            this.lbl_gpname.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pnl_gp_wrapper
            // 
            this.pnl_gp_wrapper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_gp_wrapper.Location = new System.Drawing.Point(1, 0);
            this.pnl_gp_wrapper.Name = "pnl_gp_wrapper";
            this.pnl_gp_wrapper.Size = new System.Drawing.Size(178, 70);
            this.pnl_gp_wrapper.TabIndex = 1;
            // 
            // XMenuGroup
            // 
            this.Controls.Add(this.pnl_gp_wrapper);
            this.Controls.Add(this.lbl_gpname);
            this.Name = "XMenuGroup";
            this.Size = new System.Drawing.Size(180, 88);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
