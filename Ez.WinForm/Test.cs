using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.Plug;
using Ez.WinForm.Library;
using Ez.WinForm.Properties;
using Ez.XControls.Library;
using Ez.XControls.Menus;
using Ez.XControls.xAnimate;

namespace Ez.WinForm
{
    public partial class Test : FormPage
    {
        public static int counter=0;
        //一个窗体特效，帮你了解几个windows api函数.效果：windows桌面上增加一个简单的遮罩层,其中WS_EX_TRANSPARENT 比较重要，它实现了鼠标穿透的功能
        //[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        //public static extern long GetWindowLong(IntPtr hwnd, int nIndex);
        //[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        //public static extern long SetWindowLong(IntPtr hwnd, int nIndex, long dwNewLong);
        //[DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        //private static extern int SetLayeredWindowAttributes(IntPtr Handle, int crKey, byte bAlpha, int dwFlags);
        //const int GWL_EXSTYLE = -20;
        //const int WS_EX_TRANSPARENT = 0x20;
        //const int WS_EX_LAYERED = 0x80000;
        //const int LWA_ALPHA = 2;
        public Test()
        {
            InitializeComponent();
            this.label1.Text = ":" + (counter++);
        }

        void animate_AnimationFinished(object obj, AnimateArgs<Point> args)
        {
            label2.Text="动画执行完成";
        }

        private void xButton1_Click(object sender, EventArgs e)
        {
            button1.Stop().Animate<Point>(new Point(500, 500),80, animate_AnimationFinished).Start();
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    this.BackColor = Color.Transparent;
        //    this.TopMost = true;
        //    this.FormBorderStyle = FormBorderStyle.None;
        //    this.WindowState = FormWindowState.Maximized;
        //    SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) | WS_EX_TRANSPARENT | WS_EX_LAYERED);
        //    SetLayeredWindowAttributes(Handle, 0, 128, LWA_ALPHA);
        //}

    }
}
