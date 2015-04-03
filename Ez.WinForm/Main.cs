//#define SIM
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.Dtos;
using Ez.BizContract;
using Ez.Plug;
using Ez.UI.Entities;
using Ez.WinForm;
using Ez.WinForm.Library;
using Ez.WinForm.Properties;
using Ez.XControls.Buttons;
using Ez.XControls.Library;
using Ez.XControls.Menus;
using Ez.XControls.xAnimate;
using UIModel = Ez.UI.Entities;
namespace Ez.WinForm
{
    public partial class Main : FormDefault
    {
        public Panel TARGETCONTANIER;
        /// <summary>
        /// 构造器初始化系统工作界面的入口
        /// </summary>
        /// <param name="startForm"> 程序主线程窗口程序（登录窗口）</param>
        public Main(StartForm startForm)
        {
            this.startForm = startForm;
            //初始化控件
            InitializeComponent();

            TARGETCONTANIER = pnl_body_pnl_r;
            this.Text = "收件箱-Outlook 数据文件";

            this.Deactivate += Main_Deactivate;
            this.SizeChanged += Main_SizeChanged;
            menuBar.Click += menuBar_Click;

            ForTestLoad();

        }



        void menuBar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("11");
        }

        void Main_Deactivate(object sender, EventArgs e)
        {
            ListeningAction(33);
        }

        internal void RightLayout()
        {
            //初始化各容器的尺寸位置等
            InitLayout();
        }

        #region 私有属性
        /// <summary>
        /// 程序主线程窗口程序（登录窗口）
        /// </summary>
        private StartForm startForm;
        #endregion

        #region 内部行为
        /// <summary>
        /// 切换主菜单对应的子菜单容器
        /// </summary>
        /// <param name="current">当前已被单击的Tab</param>
        private void MenuBoxChanged(Control current)
        {
            int currentindex = menuBar.Controls.IndexOf(current) - 1;
            for (int i = 0; i < this.pnl_header_bmenus.Controls.Count; i++)
            {
                Control ctrl = this.pnl_header_bmenus.Controls[i];
                if (i == currentindex)
                {
                    ctrl.Show();
                }
                else
                {
                    ctrl.Hide();
                }
            }
        }
        /// <summary>
        /// 初始化各容器的尺寸位置等（布局）
        /// </summary>
        protected override void InitLayout()
        {
            int innerMaxWidth = this.Width - 2;//1259;
            this.SuspendLayout();
            pic_ico.Image = this.Icon.ToBitmap();
            #region 尺寸&位置

            pic_help_btn.Width = pic_help_btn.Height = 13;

            pic_min_btn.Width = 13;
            pic_min_btn.Height = 13;

            pic_toggle_btn.Width = pic_toggle_btn.Height = 13;

            pic_close_btn.Width = pic_close_btn.Height = 13;

            menuBar.Width = this.Width - 1;
            menuBar.Location.Offset(new Point(-1, 0));

            pnl_header_bmenus.Width = innerMaxWidth;//宽度参考标准
            pnl_header_bmenus.Height = 96;

            pnl_header_topbar.Width = innerMaxWidth;
            pnl_header_topbar.Height = 32;

            pnl_body.Width = innerMaxWidth;
            pnl_body.Height = 585;

            pnl_bom.Width = this.Width;
            pnl_bom.Height = 22;

            pnl_header_ltop.Width = 198;
            pnl_header_ltop.Height = 27;

            pnl_header_rtop.Width = 105;
            pnl_header_rtop.Height = 32;

            pnl_header_mtop.Width = innerMaxWidth - pnl_header_ltop.Width - pnl_header_rtop.Width;
            pnl_header_mtop.Height = 27;

            pnl_body_pnl_l.Width = 230;
            pnl_body_pnl_l.Height = 580;

            pnl_body_pnl_r.Width = innerMaxWidth - pnl_body_pnl_l.Width;
            pnl_body_pnl_r.Height = 580;

            pic_help_btn.Location = new Point(10, 10);
            pic_min_btn.Location = new Point(33, 10);
            pic_toggle_btn.Location = new Point(56, 10);
            pic_close_btn.Location = new Point(79, 8);

            menuBar.Location = new Point(0, 35);
            pnl_header_bmenus.Location = new Point(1, 68);
            pnl_header_topbar.Location = new Point(1, 2);
            pnl_body.Location = new Point(1, 166);
            pnl_bom.Location = new Point(1, 751);
            pnl_header_ltop.Location = new Point(2, 2);
            pnl_header_rtop.Location = new Point(1190, 0);
            pnl_header_mtop.Location = new Point(202, 2);
            pnl_body_pnl_l.Location = new Point(2, 2);
            pnl_body_pnl_r.Location = new Point(233, 2);
            #endregion

            #region 边框&颜色
            //清空登录画面启动信息提示的文本
            startForm.lbl_starting.Text = "";
            this.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
            {
                //设置窗体边框
                Library.Utils.SetCtrlBorder(this);
            });
            base.InitLayout();

            //设置窗体指定位置可拖动窗体移动
            Library.Utils.FormMove(this, this.pnl_header_mtop, this.pnl_header_mtop_lbl_title, this.menuBar);

            //存在二级菜单 则默认显示除过默认Tab的第一个出现的Tab的子菜单
            if (this.pnl_header_bmenus.Controls.Count > 0) this.pnl_header_bmenus.Controls[0].Show();

            //绘制子菜单容器底部边框
            this.pnl_header_bmenus.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
            {
                Color borderColor = Color.FromArgb(225, 225, 225);
                ControlPaint.DrawBorder(this.pnl_header_bmenus.CreateGraphics(), this.pnl_header_bmenus.ClientRectangle,
                Color.Transparent, 0, ButtonBorderStyle.None,
                Color.Transparent, 0, ButtonBorderStyle.None,
                Color.Transparent, 0, ButtonBorderStyle.None,
                borderColor, 1, ButtonBorderStyle.Solid);
            });

            this.pnl_body_pnl_l.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
            {
                Color borderColor = Color.FromArgb(225, 225, 225);
                Graphics gp = this.pnl_body_pnl_l.CreateGraphics();
                Rectangle rect = this.pnl_body_pnl_l.ClientRectangle;
                using (Pen pen = new Pen(borderColor))
                {
                    gp.ResetClip();
                    gp.DrawLine(pen, new Point(rect.Right - 1, rect.Top - 1), new Point(rect.Right - 1, rect.Bottom - 1));
                    gp.Dispose();
                }
            });

            #endregion
#if SIM
             TestMenuFun();
#else
            int userid = 0;
            int roleid = 0;
            int.TryParse(this.GetQuery("uid").ToString(), out userid);
            int.TryParse(this.GetQuery("rid").ToString(), out roleid);
            BizResult<LayoutWinDto> result2 = BizStore.LayoutBiz.GetLayoutDataWithUserRight(userid, roleid);
            if (result2.Success)
            {
                CreateTopBar(result2.Data.TopBar);
                SideMenu(result2.Data.ShortCuts);
            }
#endif
            this.ResumeLayout();
        }


        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender">被单击对象</param>
        /// <param name="e"></param>
        private void pic_min_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 尺寸切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_toggle_btn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.pic_toggle_btn.BackgroundImage = Resources.max;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.pic_toggle_btn.BackgroundImage = Resources.normal;
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_close_btn_Click_1(object sender, EventArgs e)
        {
            this.startForm.StartPosition = FormStartPosition.CenterScreen;
            this.startForm.Close();
            this.startForm.Dispose();
            this.Close();
            this.Dispose();
        }
        #endregion

        #region 关于菜单
        private void CreateTopBar(IList<MenuBar> topBar)
        {
            menuBar.Text = "账户";


            //当tab发生单击且与上次不同时是触发的事件
            menuBar.ClickChangedEvent += new XTopBar.ClickChanged((object sender, EventArgs e) =>
            {
                MenuBoxChanged(menuBar.CurrentMenu);
            });
            if (topBar != null)
            {
                foreach (MenuBar tab in topBar)
                {
                    menuBar.AddTab(tab.Name);
                    Panel pnl_header_bmenus_pnl = new Panel();
                    pnl_header_bmenus_pnl.Height = pnl_header_bmenus.Height - 1;
                    pnl_header_bmenus_pnl.Location = new Point(0, 0);
                    pnl_header_bmenus_pnl.Width = pnl_header_bmenus.Width;
                    pnl_header_bmenus_pnl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                    int x = 0, i = 0;
                    foreach (MGroup gp in tab.Children)
                    {
                        XMenuGroup xMenuGroup = new XMenuGroup() { GroupName = gp.Name };
                        x = i > 0 ? (pnl_header_bmenus_pnl.Controls[i - 1].Location.X + pnl_header_bmenus_pnl.Controls[i - 1].Width) : 0;
                        xMenuGroup.Location = new Point(x, 0);
                        i++;
                        foreach (UIModel.Menu btn in gp.Children)
                        {
                            Image img = null;
                            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imgs", btn.Ico ?? "");
                            if (File.Exists(path))
                            {
                                img = Image.FromFile(path);
                            }
                            else
                            {
                                img = Resources.email;
                            }

                            XButton xbtn = new XButton(GetCtrlInstance)
                            {
                                Image = img,
                                Text = btn.Name,
                                Width = 55,
                                Height = 70,
                                Child = CreateXmenu(btn.Children, true),
                                Src = btn.Url,
                                TargetContainer = pnl_body_pnl_r
                            };
                            xMenuGroup.AddControl(xbtn);
                        }
                        pnl_header_bmenus_pnl.Controls.Add(xMenuGroup);
                    }
                    pnl_header_bmenus.Controls.Add(pnl_header_bmenus_pnl);
                }
            }
        }
        private XMenu CreateXmenu(IList<MenuGroup> menus, bool isbottom)
        {
            XMenu xmenu = null;
            if (menus != null && menus.Count > 0)
            {
                xmenu = new XMenu() { IsBottomPos = isbottom };
                for (int i = 0; i < menus.Count; i++)
                {
                    MenuGroup mgp = menus[i];
                    foreach (UIModel.Menu item in mgp)
                    {
                        xmenu.AddItem(new XMenuItem(GetCtrlInstance)
                        {
                            Name = "item_" + item.GetHashCode(),
                            Ico = Resources.semail,
                            Text = item.Name,
                            Height = 25,
                            TargetContainer = pnl_body_pnl_r,
                            Src = item.Url,
                            Child = CreateXmenu(item.Children, false)
                        });
                    }
                    if (i != menus.Count - 1)
                    {
                        xmenu.AddItem(new XMenuItem() { Text = "-" });
                    }
                }
            }
            return xmenu;
        }
        private void SideMenu(ShortCutCollection shortCuts)
        {
            SideItem sidebaritem = new SideItem() { ItemTitle = "快捷菜单", Children = new SideItemChildren() };

            if (shortCuts != null)
            {
                foreach (ShortCut item in shortCuts)
                {
                    sidebaritem.Children.Add(new XMenuItem(GetCtrlInstance)
                    {
                        Height = 25,
                        Ico = Resources.semail,
                        Src = item.Url,
                        TargetContainer = TARGETCONTANIER,
                        Text = item.Name
                    });
                }
            }
            xSideMenuBar1.AddItem(sidebaritem);
        }
        #endregion


        /// <summary>
        /// 设置窗口标题
        /// </summary>
        public override string Text
        {
            get
            {
                if (pnl_header_mtop_lbl_title != null)
                {
                    return pnl_header_mtop_lbl_title.Text;
                }
                else
                {
                    return base.Text;
                }
            }
            set
            {
                base.Text = pnl_header_mtop_lbl_title.Text = value;
            }
        }


        private void ForTestLoad()
        {
            O = new Point(panel1.Location.X + panel1.Size.Width / 2, panel1.Location.Y + panel1.Size.Height / 2);
            xSideMenuBar1.AddItem(new SideItem()
            {
                ItemTitle = "标题菜单2",
                Children = new SideItemChildren{
                       new XMenuItem(GetCtrlInstance){ Height=25, Ico =Resources.semail, Src = "http://www.diandian.com", TargetContainer = TARGETCONTANIER,Text="点点网"},
                       new XMenuItem(GetCtrlInstance){ Height=25, Ico =Resources.semail, Src = "http://www.jidianzaixian.com", TargetContainer = TARGETCONTANIER,Text="祭奠在线"},
                       new XMenuItem(GetCtrlInstance){ Height=25, Ico =Resources.semail, Src = "http://www.baidu.com", TargetContainer = TARGETCONTANIER,Text="百度网"},
                       new XMenuItem(GetCtrlInstance){ Height=25, Ico =Resources.semail, Src = "http://www.weibo.com", TargetContainer = TARGETCONTANIER,Text="微博"},
                       new XMenuItem(GetCtrlInstance){ Height=25, Ico =Resources.semail, Src = "win://WinForm(Ez.WinForm.Views.Account)/index?uid=123&123f=f", TargetContainer = TARGETCONTANIER,Text="子菜单4"}
              }

            });
        }
#if SIM

        /// <summary>
        /// 测试，以后从数据模型来
        /// </summary>
        private void TestMenuFun()
        {

        #region 构造一个五级50菜单的内容
            XMenu _xMenu_lv50 = new XMenu() { IsBottomPos = false };
            _xMenu_lv50.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "菜单501",
                Height = 25,
                TargetContainer = pnl_body_pnl_r,
                Src = new Test("被五级菜单51按钮调用的窗体")
            });
            _xMenu_lv50.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "菜单502",
                Height = 25,
                TargetContainer = pnl_body_pnl_r,
                Src = new Test("被五级菜单502按钮调用的窗体")
            });
        #endregion

        #region 构造一个五级51菜单的内容
            XMenu _xMenu_lv51 = new XMenu() { IsBottomPos = false };
            _xMenu_lv51.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "菜单511",
                Height = 25,
                TargetContainer = pnl_body_pnl_r,
                Src = new Test("被五级菜单511按钮调用的窗体")
            });
            _xMenu_lv51.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "菜单512",
                Height = 25,
                TargetContainer = pnl_body_pnl_r,
                Src = new Test("被五级菜单512按钮调用的窗体")
            });
        #endregion

        #region 构造一个四40级菜单的内容

            XMenu _xMenu_lv40 = new XMenu() { IsBottomPos = true };
            _xMenu_lv40.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "菜单401",
                Height = 25,
                TargetContainer = pnl_body_pnl_r,
                Src = new Test("被五级菜单511按钮调用的窗体")
                //Child = _xMenu_lv50
            });
            _xMenu_lv40.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "菜单402",
                Height = 25,
                Child = _xMenu_lv51
            });
        #endregion

        #region 构造一个四41级菜单的内容
            XMenu _xMenu_lv41 = new XMenu() { IsBottomPos = true };

            _xMenu_lv41.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "菜单41",
                Height = 25,
                Child = _xMenu_lv50
            });
            _xMenu_lv41.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "其他41",
                Height = 25,
                Child = _xMenu_lv51
            });
            _xMenu_lv41.AddItem(new XMenuItem()
            {
                Ico = Resources.ico,
                Text = "其他-41",
                Height = 25,
                Src = new Test("123"),
                TargetContainer = pnl_body_pnl_r
            });
        #endregion

        #region 构造一个顶级菜单
            string[] topMenuName = new string[] { "开始", "发送/接受", "文件夹", "视图" };
            foreach (string topname in topMenuName)
            {
                menuBar.AddTab(topname);
            }
            //当tab发生单击且与上次不同时是触发的事件
            menuBar.ClickChangedEvent += new XTabBar.ClickChanged((object sender, EventArgs e) =>
            {
                MenuBoxChanged(menuBar.CurrentMenu);
            });
        #endregion

        #region 构造二、三级菜单
            //新建按钮（二级菜单）对应的菜单容器
            Panel pnl_header_bmenus_pnl0 = new Panel();
            pnl_header_bmenus_pnl0.Height = pnl_header_bmenus.Height - 1;
            //二级菜单
            string[] names = new string[] { "新建", "删除", "快速步骤", "移动", "标记" };
            int x = 0;
            for (int i = 0; i < names.Length; i++)
            {
                XMenuGroup xMenuGroup = new XMenuGroup() { GroupName = names[i] };
                for (int t = 0; t < 4; t++)
                {
        #region  构造一个组的菜单项（三级菜单）
                    XMenu menu = null;
                    string text = "电子邮件";
                    if (i == 0 && (t == 0 || t == 1))
                    {
                        menu = t == 0 ? _xMenu_lv40 : _xMenu_lv41;
                    }
                    else
                    {
                        text = "  新建\r\n电子邮件";
                    }
                    XButton xbtn = new XButton()
                    {
                        Image = Resources.email,
                        Text = text,
                        Width = 55,
                        Height = 70,
                        Tag = t,
                        Child = menu,
                        Src = new Test("被按钮" + t + "调用的窗体"),
                        TargetContainer = pnl_body_pnl_r
                    };
                    xMenuGroup.AddControl(xbtn);
        #endregion
                }
                x = i > 0 ? (pnl_header_bmenus_pnl0.Controls[i - 1].Location.X + pnl_header_bmenus_pnl0.Controls[i - 1].Width) : 0;
                xMenuGroup.Location = new Point(x, 0);

                pnl_header_bmenus_pnl0.Controls.Add(xMenuGroup);
            }
        #endregion

            pnl_header_bmenus_pnl0.Location = new Point(0, 0);
            pnl_header_bmenus_pnl0.Width = pnl_header_bmenus.Width;
            pnl_header_bmenus_pnl0.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pnl_header_bmenus.Controls.Add(pnl_header_bmenus_pnl0);

        }
#endif
        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern int GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        IList<XMenu> mutil_xmenus = new List<XMenu>();
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            ListeningAction(m.Msg);
        }

        private void ListeningAction(int msg)
        {
            switch (msg)
            {
                case 33:
                    Point defPnt = new Point();
                    GetCursorPos(ref defPnt);
                    defPnt.X = defPnt.X - this.Location.X;
                    defPnt.Y = defPnt.Y - this.Location.Y;
                    bool display = true;
                    foreach (Control ctrl in this.Controls)
                    {
                        if (ctrl is XMenu)
                        {
                            XMenu xmenu = ctrl as XMenu;
                            if (!mutil_xmenus.Contains(xmenu))
                            {
                                mutil_xmenus.Add(xmenu);
                            }
                            Rectangle rect = new Rectangle(ctrl.Location, xmenu.Size);
                            if (rect.Contains(defPnt) && display)
                            {
                                display = false;
                                break;
                            }
                        }
                    }
                    if (display)
                    {
                        foreach (XMenu item in mutil_xmenus)
                        {
                            item.Hide();
                        }
                    }
                    break;
            }

        }

        Point O;
        private void button1_Click_1(object sender, EventArgs e)
        {
            Animate<Size> animate = panel1.Stop().Animate<Size>(new Size(800, 800), 100, animate2_AnimationFinished);
            animate.StepSizeChanged += animate2_StepSizeChanged;
            animate.Start();
        }

        void animate2_AnimationFinished(object obj, AnimateArgs<Size> args)
        {
            Animate<Size> animate = panel1.Stop().Animate<Size>(new Size(300, 300), 100);
            animate.StepSizeChanged += animate2_StepSizeChanged;
            animate.Start();
        }
        void animate2_StepSizeChanged(object obj, AnimateArgs<Size> args)
        {
            panel1.Location = new Point(O.X - (args.CurrentValue.Width / 2), O.Y - (args.CurrentValue.Height / 2));
        }

        bool opened = true;
        Point oldPointL = Point.Empty;
        Point oldPointR = Point.Empty;
        int offsetX = 0;
        int oldWidthR = 0;
        int oldWidthL = 0;
        Size old_winSize = Size.Empty;
        void Main_SizeChanged(object sender, EventArgs e)
        {
            if (oldPointL == Point.Empty && oldPointR == Point.Empty)
            {
                oldPointL = this.pnl_body_pnl_l.Location;
                oldPointR = this.pnl_body_pnl_r.Location;
                oldWidthL = this.pnl_body_pnl_l.Size.Width;
                oldWidthR = this.pnl_body_pnl_r.Size.Width;
                offsetX = oldWidthL - 13;
                old_winSize = this.Size;
            }
        }
        private void xbtn_sideSwitch_Click(object sender, EventArgs e)
        {
            opened = !opened;
            int difw = this.Size.Width - old_winSize.Width;
            int difh = this.Size.Height - old_winSize.Height;
            if (opened)
            {
                this.pnl_body_pnl_l.Location = oldPointL;
                this.pnl_body_pnl_r.Location = oldPointR;
                this.pnl_body_pnl_r.Size = new Size(oldWidthR + difw, this.pnl_body_pnl_r.Size.Height + difh);
            }
            else
            {
                this.pnl_body_pnl_l.Location = new Point(-offsetX, this.pnl_body_pnl_l.Location.Y);
                this.pnl_body_pnl_r.Location = new Point(offsetX, this.pnl_body_pnl_r.Location.Y);
                this.pnl_body_pnl_r.Size = new Size(oldWidthR + offsetX + difw, this.pnl_body_pnl_r.Size.Height + difh);
                
            }
            

           

            /*
            Animate<Point> animate = this.pnl_body_pnl_l.Stop().Animate<Point>(currentPoint, 20,(object obj, AnimateArgs<Point> args) =>
            {
                this.xbtn_sideSwitch.Image = colsed ? Resources.switchr : Resources.switchl;
             
            });
            animate.StepSizeChanged += animate_StepSizeChanged;
            animate.Start();
            */
        }
        
        void animate_StepSizeChanged(object obj, AnimateArgs<Point> args)
        {
            //this.pnl_body_pnl_r.Size = new Size(rightWidth - args.CurrentValue.X, this.pnl_body_pnl_r.Size.Height);
            //this.pnl_body_pnl_r.Location = new Point(args.CurrentValue.X + this.pnl_body_pnl_l.Width, args.CurrentValue.Y);
        }
    }
}
