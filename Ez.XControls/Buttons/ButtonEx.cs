using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.XControls.Library;

namespace Ez.XControls.Buttons
{
    [ToolboxItem(true)]
    public class ButtonEx : System.Windows.Forms.Button
    {
        /// <summary> 
        /// 是否启用热点效果 
        /// </summary> 
        private bool _HotTrack = true;

        /// <summary> 
        /// 
        /// </summary> 
        public ButtonEx()
            : base()
        {
            this.BackColor = Color.FromArgb(6265547);
        }
        /// <summary> 
        /// 鼠标移动到该控件上时 
        /// </summary> 
        protected override void OnMouseMove(MouseEventArgs e)
        {

            if (this._HotTrack)
            {
                this.Invalidate();
            }
            base.OnMouseMove(e);
        }
        /// <summary> 
        /// 当鼠标从该控件移开时 
        /// </summary> 
        /// <param name="e"></param> 
        protected override void OnMouseLeave(EventArgs e)
        {


            if (this._HotTrack)
            {
                //重绘 
                this.Invalidate();
            }
            base.OnMouseLeave(e);
        }
        /// <summary> 
        /// 当该控件获得焦点时 
        /// </summary> 
        /// <param name="e"></param> 
        protected override void OnGotFocus(EventArgs e)
        {

            if (this._HotTrack)
            {
                //重绘 
                this.Invalidate();
            }
            base.OnGotFocus(e);
        }
        /// <summary> 
        /// 当该控件失去焦点时 
        /// </summary> 
        /// <param name="e"></param> 
        protected override void OnLostFocus(EventArgs e)
        {
            if (this._HotTrack)
            {
                //重绘 
                this.Invalidate();
            }
            base.OnLostFocus(e);
        }
        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);

            if (m.Msg == 0xf || m.Msg == 0x133)//
            {
                /*
                 * 0x133:当一个编辑型控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以 通过使用给定的相关显示设备的句柄来
                 * 设置编辑框的文本和背景颜色 
                 * 0xf
                 */
                if (this.FlatStyle == System.Windows.Forms.FlatStyle.Flat)
                {
                    IntPtr hDC = Utils.GetWindowDC(m.HWnd);
                    if (hDC.ToInt32() == 0)
                    {
                        return;
                    }
                    //边框Width为1个像素 
                    Pen pen = new Pen(Utils.BorderColor, 1);
                    //绘制边框 
                    System.Drawing.Graphics g = Graphics.FromHdc(hDC);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                    pen.Dispose();

                    //返回结果 
                    m.Result = IntPtr.Zero;
                    //释放 
                    Utils.ReleaseDC(m.HWnd, hDC);
                }
            }
        }
    }
}
