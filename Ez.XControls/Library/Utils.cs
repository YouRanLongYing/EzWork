using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ez.XControls.Library
{
    public class Utils
    {
        /// <summary> 
        /// 窗体的边框颜色 
        /// </summary> 
        public static Color BorderColor = Color.FromArgb(84, 159, 212);
        /// <summary> 
        /// 热点边框颜色 
        /// </summary> 
        public static Color HotColor = Color.FromArgb(0, 114, 198);
        /// <summary> 
        /// 获得当前进程，以便重绘控件 
        /// </summary> 
        /// <param name="hWnd"></param> 
        /// <returns></returns> 
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// 禁止控件重绘
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;

        /// <summary>
        /// 获取控件的顶级容器
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static Form TopContainer(Control ctrl)
        {
            Control top = ctrl;
            if (ctrl.Parent.Parent == null)
            {
                top = ctrl.Parent;
            }
            else
            {
                top = TopContainer(ctrl.Parent);
            }
            return top as Form;
        }

        /// <summary>
        /// 停止控件的重绘
        /// </summary>
        /// <param name="handle"></param>
        public static void BeginPaint(IntPtr handle)
        {
            SendMessage(handle, WM_SETREDRAW, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 允许控件重绘.
        /// </summary>
        /// <param name="handle"></param>
        public static void EndPaint(IntPtr handle)
        {
            SendMessage(handle, WM_SETREDRAW, 1, IntPtr.Zero);
        }

  
        public void Animate(object args,int time,Func<bool> func)
        {
            


        }
    }
}
