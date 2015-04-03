using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ez.Dtos;
using Ez.Controllers.Library;
using Ez.BizContract;
using Ez.Core;
using Ez.UI;
using Ez.Lang;
using Ez.UI.HtmlExtend;
using Ez.Config;
using Ez.Cache;
using Ez.Helper;
using Ez.Payment.Contract;
using Ez.UI.Entities;
namespace Ez.Controllers
{
    [Authentication(AuthenticationEnum.Identity)]
    public partial class WindowController : DefaultController<IAccountBiz>
    {
        /// <summary>
        /// 系统布局数据
        ///  create by kongjing
        /// </summary>
        public ILayoutBiz LayoutBiz { set; get; }

        /// <summary>
        /// 初始化登录
        ///  create by kongjing
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {

            return View();
        }

        /// <summary>
        /// 提交登录
        ///  create by kongjing
        /// </summary>
        [HttpPost]
        [AsyncAction]
        public JsResult AsyncLogin(LoginInfoDto dto)
        {
            this.CurrentUser = DefaultBiz.Login(dto);
            if (this.CurrentUser != null && this.CurrentUser.id > 0)
            {
                SessionProxy.Instance.SetLoginInfo(this.CurrentUser);
                string redirect = string.IsNullOrEmpty(Request["returnUrl"]) ? "/" : Request["returnUrl"];
                return new JsResult(
                    new { id = this.CurrentUser.id, loginName = this.CurrentUser.login_name, lastLoginTime = this.CurrentUser.last_loginTime, lastLoginIp = this.CurrentUser.last_login_ip },
                    true, redirect);
            }
            else
            {
                return new JsResult(false, null, EzLanguage.COM_Msg_AccountError);
            }
        }

        /// <summary>
        /// 登录出操作
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public RedirectResult LoginOut(string redirectUrl)
        {
            SessionProxy.Instance.Abandon();
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return Redirect(redirectUrl);
            }
            else
            {
                return Redirect(UIConfig.Model.Login ?? "Login"); 
            }
        }

        /// <summary>
        /// 默认首页
        ///  create by kongjing
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(UIConfig.Model.LayoutAction, GetWindowDto(UIConfig.Model.LayoutAction.ToLower() == "desktop").Data);
        }

        /// <summary>
        /// 桌面风格首页
        ///  create by kongjing
        /// </summary>
        /// <returns></returns>
        public ActionResult Desktop()
        {
            return View(GetWindowDto(true).Data);
        }

        /// <summary>
        /// 传统风格首页
        ///  create by kongjing
        /// </summary>
        /// <returns></returns>
        public ActionResult Tradition()
        {
            return View(GetWindowDto(false).Data);
        }

        public ActionResult Tradition_mobile()
        {
            return View(GetWindowDto(false).Data);
        }

        /// <summary>
        /// 切换系统语言
        ///  create by kongjing
        /// </summary>
        /// <param name="lang"></param>
        public void ChangeLanuage(string lang)
        {
            if (UIConfig.Model.AvailableCultures.Any(p => p.Key.Equals(lang)))
            {
                System.Web.HttpContext.Current.Session[Constans.SYS_LANAGUE_KEY] = lang;
            }
            Uri uri = System.Web.HttpContext.Current.Request.UrlReferrer;
            Response.Redirect(uri != null ? "/" : uri.AbsolutePath);
            //throw new Exception("系统不支持要切换的语言："+lang);
        }
    }
    /// <summary>
    /// 私有成员
    /// </summary>
    public partial class WindowController
    {
        [NonAction]
        private BizResult<WindowDto> GetWindowDto(bool isdesktop)
        {
            if (this.CurrentUser.CurrentRole == null) return new BizResult<WindowDto>(false, null);
            BizResult<WindowDto> result = this.LayoutBiz.GetLayoutInfoWithUserRight(this.CurrentUser.id,this.CurrentUser.CurrentRole.id);//this.BizManager.GetLayoutInfoWithUserRight(this.CurrentUser.id);
            if (result.Data != null)
            {
                result.Data.LoginInfo = this.CurrentUser;
                //result.Result.TopBar.TopMenus.Add(new TopMenu(EzLanguage.COM_N_Window, new { id = Constans.OPENED_WINDOW_LIST_ID }));
                var group = new MenuGroup();
                foreach (ShortCut appbtn in result.Data.ShortCuts)
                {
                    group.Add(appbtn as Menu);
                }
                result.Data.TopBar.TopMenus.Add(new TopMenu(EzLanguage.COM_N_Shortcut) { Id = "quick", Children = new List<MenuGroup> { group } });
                if (isdesktop) result.Data.TopBar.TopMenus.Add(new TopMenu(EzLanguage.COM_N_Window, new { href = "javascript:void(0)", id = Constans.OPENED_WINDOW_LIST_ID }));
                result.Data.TopBar.TopMenus.Add(new TopMenu("JqueryAPI") { WinTitle = "jQuery 在线手册 | jQuery API 中文手册 | jQuery api 1.11.1", IsWin = true, Url = "http://jquery.cuishifeng.cn/", WinSize = WinSizeEnum.Max });
                result.Data.TopBar.TopMenus.Add(new TopMenu("jqGrid Demos") { WinTitle = "jqGrid Demos", IsWin = true, Url = "http://www.trirand.com/blog/jqgrid/jqgrid.html", WinSize = WinSizeEnum.Max });
            }
            else if (UIConfig.Model.AvailableCultures.Keys.Count() > 0)
            {
                result.Data = new WindowDto
                {
                    LoginInfo = this.CurrentUser,
                    TopBar = new TopBar()
                };
                result.Data.TopBar.TopMenus = new List<TopMenu>();
                TopMenu topmenu = new TopMenu(EzLanguage.COM_N_ChangeLang)
                {
                    Children = new List<MenuGroup>()
                };
                result.Data.TopBar.TopMenus.Add(topmenu);
                MenuGroup group = new MenuGroup();
                foreach (string key in UIConfig.Model.AvailableCultures.Keys)
                {
                    string lang_name = UIConfig.Model.AvailableCultures[key];
                    Menu lang_menu = new Menu
                    {
                        Id = "changlang",
                        Name = lang_name,
                        Url = "/window/ChangeLanuage?lang=" + key,
                        IsWin = false
                    };
                    if (key.Equals(System.Web.HttpContext.Current.Session[Constans.SYS_LANAGUE_KEY]))
                    {
                        lang_menu.Attributes = new { @class = "selected" };
                    }
                    group.Add(lang_menu);
                }
                topmenu.Children.Add(group);
            }
            //result.Result.TopBar.TopMenus.Add(new TopMenu(EzLanguage.COM_N_ChangeLang){});
            //result.Result.TopBar.TopMenus.Add(new TopMenu(EzLanguage.ResourceManager.GetString("CustomKey")){});
            return result;
        }
    }
    /// <summary>
    /// 测试数据
    /// </summary>
    public partial class WindowController
    {
        protected virtual ShortCutCollection GetAppBtns()
        {
            return new ShortCutCollection(53, 27) { 
                  new ShortCut{Ico="big.png",Name = "WeiBo", Url="http://cn.bing.com", IsWin =true, WinSize= WinSizeEnum.Max, UseLeftMenu =true, UseTopSearchBar=false},
                  new ShortCut{ Ico ="diskexplorer.png",Name="DiskExplorer",Url="http://www.weiyun.com/disk/index.html?WYTAG=weiyun.portal.index",IsWin=true, WinSize= WinSizeEnum.Normal,UseLeftMenu=false,UseTopSearchBar=true},
                  new ShortCut{ Ico ="web3d.png",Name="地下管线场景",Url="http://192.168.35.70:8001/",IsWin=true, WinSize= WinSizeEnum.Max,UseLeftMenu=false,UseTopSearchBar=false}
            };
        }

        protected virtual TopBar GetTopBar(ShortCutCollection appbtns)
        {
            var group = new MenuGroup();
            foreach (ShortCut appbtn in appbtns)
            {
                group.Add(appbtn as Menu);
            }

            return new TopBar
            {
                TopMenus = new List<TopMenu>
                    {
                      new TopMenu(EzLanguage.COM_N_Menu){
                       Children= new List<MenuGroup>{
                           new MenuGroup{
                                new Menu{
                                Name = EzLanguage.COM_N_SystemSet,
                                Attributes=null,
                                Children = new List<MenuGroup>{
                                   new MenuGroup{
                                       new Menu{
                                          Name =EzLanguage.COM_N_SystemBaseInfo,
                                          Attributes=null,
                                          Children =null,
                                          Sort =0
                                       },
                                       new Menu{
                                          Name =EzLanguage.COM_N_DeskBg,
                                          Attributes=null,
                                          Children =null,
                                          Sort =1
                                       }
                                   },
                                   new MenuGroup{
                                       new Menu{
                                          Name =EzLanguage.COM_N_ChangeLang,
                                          Attributes=null,
                                          Children =null,
                                          Sort =0
                                       }
                                   }
                                }
                              }
                           },
                           new MenuGroup{
                               new Menu{
                                    Name=EzLanguage.COM_N_AccountManage,
                                    Attributes=null,
                                    Children=new List<MenuGroup>
                                    {
                                       new MenuGroup{
                                         new Menu{
                                          Name = EzLanguage.COM_N_AccountInfo,
                                          //Attributes=new{href="javascript:void(0)",data_src="/UCenter/EditMyInfo",data_iswin =true,data_win_size="max"},
                                          Children = null,
                                          Sort =0,
                                          Url = "/UCenter/EditMyInfo",
                                          IsWin = true,
                                          WinSize = WinSizeEnum.Max,
                                          UseLeftMenu =true,
                                          UseTopSearchBar =false
                                         },
                                         new Menu{
                                            Name = EzLanguage.COM_N_ChangePwd,
                                            Attributes=null,
                                            Children =null,
                                            Sort=1
                                         }
                                       }
                                    }
                                  }
                         },
                         new MenuGroup{
                           new Menu{ Name="墓园管理系统",Url="http://192.168.35.61:8096",IsWin=true,WinSize=WinSizeEnum.Max}
                         }
                       }
                      },
                      new TopMenu(EzLanguage.COM_N_Shortcut){Children = new List<MenuGroup>{group}},
                      new TopMenu(EzLanguage.COM_N_Window,new{href="javascript:void(0)",id=Constans.OPENED_WINDOW_LIST_ID}),
                      new TopMenu("JqueryAPI"){ WinTitle="jQuery 在线手册 | jQuery API 中文手册 | jQuery api 1.11.1",IsWin=true, Url="http://jquery.cuishifeng.cn/", WinSize=WinSizeEnum.Max}
                    }
            };
        }
    }
}
