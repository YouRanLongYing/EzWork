using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.BizContract;
using Ez.Plug;
using Ez.WinForm.Library;
namespace Ez.WinForm
{
    public partial class StartForm : FormDefault
    {
        delegate void StartInfo();


        public StartForm()
        {
            InitializeComponent();
            InitWindow();
        }
        private void InitWindow()
        {
            this.Paint += Form1_Paint;
            Utils.FormMove(this);
            this.lbl_starting.TextAlign = ContentAlignment.MiddleCenter;
            this.lbl_starting.Text = string.Empty;
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.BackColor = Color.FromArgb(239, 239, 242);
            Utils.SetCtrlBorder(this);
        }
        private void LoadStart(object userid_roleid)
        {
            Thread.Sleep(1000);
            this.lbl_starting.BeginInvoke(new Func<int>(() =>
            {
                this.lbl_starting.ForeColor = lblcolor.Value;
                this.lbl_starting.Text = "正在配置";
                return 0;
            }));
            Thread.Sleep(1000);
            this.BeginInvoke(new StartInfo(() =>
            {
                Main main = new Main(this);
                string[] ids = userid_roleid as string[];
                if (ids == null || ids.Length != 2) MessageBox.Show("账户错误无法进入");
                main.SetQuery("uid", ids[0]);
                main.SetQuery("rid", ids[1]);

                main.StartPosition = FormStartPosition.CenterScreen;
                main.WindowState = FormWindowState.Normal;
                main.RightLayout();
                main.Show();
                this.Hide();
            }));

        }
        Color? lblcolor = null;
        private void btn_login_Click(object sender, EventArgs e)
        {
            if (lblcolor == null) lblcolor = this.lbl_starting.ForeColor;
            this.lbl_starting.Text = "验证中";

            Dtos.LoginInfoDto login_info = BizStore.AccountBiz.Login(new Dtos.LoginInfoDto
            {
                login_name = this.tbx_uid.Text.Trim(),
                password = this.tbx_pwd.Text.Trim()
            });
            if (login_info!=null&&login_info.CurrentRole == null)
            {
                this.lbl_starting.ForeColor = Color.Red;
                this.lbl_starting.Text = "此用户无任何权限进入";
            }
            else
            {
                if (login_info != null)
                {
                    this.lbl_starting.Text = "验证成功!";
                    Thread thrad = new Thread(new ParameterizedThreadStart(LoadStart));
                    thrad.Start(new string[] { login_info.id.ToString(), login_info.CurrentRole.id.ToString() });
                }
                else
                {
                    this.lbl_starting.ForeColor = Color.Red;
                    this.lbl_starting.Text = "账户或密码错误！";
                }
            }
        }

        private void lbl_close_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Application.ExitThread();
            Application.Exit();
            BizStore.Dispose();
        }
    }

}
