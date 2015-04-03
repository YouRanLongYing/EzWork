using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.Plug;

namespace Ez.WinForm.Library
{
    public static class Utils
    {
        [DllImport("User32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("User32.DLL")]
        public static extern bool ReleaseCapture();
        private const uint WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 61456;
        private const int HTCAPTION = 2;

        public static Color WinBorderColor = Color.FromArgb(0, 114, 198);
        public static Color NormalColor = Color.FromArgb(240, 240, 240);

        public static void FormMove(Form window)
        {
            window.MouseDown += (object sender, MouseEventArgs e) =>
            {
                ReleaseCapture();
                SendMessage(window.Handle, WM_SYSCOMMAND, SC_MOVE | HTCAPTION, 0);
            };
        }
        public static void FormMove(Form window,params Control[] fireCtrls)
        {
            foreach (Control ctrl in fireCtrls)
            {
                ctrl.MouseDown += (object sender, MouseEventArgs e) =>
                {
                    ReleaseCapture();
                    SendMessage(window.Handle, WM_SYSCOMMAND, SC_MOVE | HTCAPTION, 0);
                };
            }
        }
        public static void SetCtrlBorder(Control ctrl,Color? color=null)
        {
            if (color == null)
            {//设置边框颜色
                color = Color.FromArgb(0, 114, 198);
            }
            ControlPaint.DrawBorder(ctrl.CreateGraphics(), ctrl.ClientRectangle,
               color.Value, 1, ButtonBorderStyle.Solid,
               color.Value, 1, ButtonBorderStyle.Solid,
               color.Value, 1, ButtonBorderStyle.Solid,
               color.Value, 1, ButtonBorderStyle.Solid);
        }
        public static void Metro(Control ctrl)
        {

            if (ctrl is TextBox)
            {
                TextBox txt = ctrl as TextBox;
                if (txt == null) return;
                txt.BorderStyle = BorderStyle.FixedSingle;

                //txt.ClientRectangle.Inflate(5, 5);
                ControlPaint.DrawBorder(txt.CreateGraphics(), txt.ClientRectangle,
                   NormalColor, 1, ButtonBorderStyle.Solid,
                   NormalColor, 1, ButtonBorderStyle.Solid,
                   NormalColor, 1, ButtonBorderStyle.Solid,
                   NormalColor, 1, ButtonBorderStyle.Solid);

                txt.MouseHover += (object sender, EventArgs e) =>
                {
                    ControlPaint.DrawBorder(txt.CreateGraphics(), txt.ClientRectangle,
                    WinBorderColor, 1, ButtonBorderStyle.Solid,
                    WinBorderColor, 1, ButtonBorderStyle.Solid,
                    WinBorderColor, 1, ButtonBorderStyle.Solid,
                    WinBorderColor, 1, ButtonBorderStyle.Solid);
                };

            }

        }


        public static Control GetFormInstance(string uri,TransData transData)
        {
            //匹配参数
            string pattern = @"win://(\S+)\((.+)\)/(\w+)\??(.+)?";

            Regex regx = new Regex(pattern, RegexOptions.IgnoreCase);

            if (!string.IsNullOrEmpty(uri) && regx.IsMatch(uri))
            {
                Match match = regx.Match(uri);

                string assembyName = match.Groups[1].Value;
                string namespaceStr = match.Groups[2].Value;
                string formClassName = match.Groups[3].Value;

                Type t = Type.GetType(string.Format("{0}.{1},{2}", namespaceStr, formClassName, assembyName));
                if (t == null) return null;
                string[] pa = new string[] { };
                object dObj = Activator.CreateInstance(t, pa);
                if (dObj is FormBase)
                {
                    FormBase form = dObj as FormBase;
                    if (match.Groups.Count == 5 && dObj != null)
                    {
                        string param = match.Groups[4].Value;
                        form.InjectQuery(param);
                    }
                    return form;
                }
                else if (dObj is IWinPlug)
                {
                    IWinPlug plug = dObj as IWinPlug;
                    //this.Text = plug.PlugName;
                    Form form = plug.Instance(transData);
                    form.TopLevel = false;
                    form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;
                    return form;
                }
                else if (dObj is Form)
                {
                    Form form = dObj as Form;
                    form.TopLevel = false;
                    form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;
                    return form;
                }
            }
            else if (uri != null && Regex.IsMatch(uri,@"^http://.+"))
            {
                WebBrowser wb = new WebBrowser();
                wb.Dock = DockStyle.Fill;
                wb.Url = new Uri(uri);
                return wb;
            }
            return null;
        }
    }
}
