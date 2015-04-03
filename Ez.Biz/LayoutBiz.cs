using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Ez.BizContract;
using Ez.Dtos.Entities;
using Ez.Dtos;
using Ez.Helper;
using Ez.UI.HtmlExtend;
using Ez.Core;
using Ez.Config;
using Ez.DBContract;
using Ez.UI.Entities;


namespace Ez.Biz
{
    /// <summary>
    /// 框架前端UI布局数据管理器
    /// </summary>
    public partial class LayoutBiz : DefaultBiz, ILayoutBiz
    {
        /// <summary>
        /// 获取可授权的功能模块，用于布局
        /// </summary>
        /// <param name="login_id">当前登录用户</param>
        /// <param name="roleid">角色编号</param>
        /// <returns></returns>
        public BizResult<WindowDto> GetLayoutInfoWithUserRight(int login_id,int roleid)
        {
            BizResult<WindowDto> result = new BizResult<WindowDto>();
            BizResult<IList<FW_U_Rights>> bizresult = this.RoleBiz.GetRights(login_id, roleid,true);
            if (result.Success=bizresult.Success)
            {
                IList<FW_U_Rights> rights = bizresult.Data;
                WindowDto windto = new WindowDto();
                windto.TopBar = GetTopBar(rights);
                windto.UserInfo = this.AccountBiz.GetUserInfo(login_id);
                windto.LoginInfo = this.AccountBiz.GetLoginInfo(login_id);
                windto.ShortCuts = new ShortCutCollection(53, 27);
                IEnumerable<FW_U_Rights> short_cuts = rights.Where(p => p.is_shortcut && !p.parent_id.Equals(0));
                foreach (FW_U_Rights item in short_cuts)
                {
                    windto.ShortCuts.Add(new ShortCut
                    {
                        Id = item.id.ToString(),
                        IsWin = item.is_win,
                        Name = Tools.GetResourceString(item.right_name_rs_key, item.right_name),
                        Ico = string.IsNullOrEmpty(item.ico) ? "none.jpg" : item.ico,
                        Url = item.limit_url,
                        WinSize = (WinSizeEnum)item.win_size,
                        Win_Height = item.win_height,
                        Win_Width = item.win_width,
                        WinTitle = Tools.GetResourceString(item.title_rs_key, item.title)
                    });
                }
                var user_shortcuts = this.GetShortCuts(login_id);
                foreach (FW_U_Shortcut item in user_shortcuts)
                {
                    windto.ShortCuts.Add(new ShortCut
                    {
                        Id = item.id.ToString(),
                        IsWin = item.is_win,
                        Name = Tools.GetResourceString(item.shortcut_name_rs_key, item.shortcut_name),
                        Ico = string.IsNullOrEmpty(item.ico) ? "none.jpg" : item.ico,
                        Url = item.url,
                        WinSize = (WinSizeEnum)item.win_size,
                        Win_Height = item.win_height,
                        Win_Width = item.win_width
                    });
                }
                result.Data = windto;
                
            }
            return result;
        }
        /// <summary>
        /// 获取可授权的功能模块，用于布局
        /// </summary>
        /// <param name="login_id">当前登录用户</param>
        /// <param name="roleid">角色编号</param>
        /// <returns></returns>
        public BizResult<LayoutWinDto> GetLayoutDataWithUserRight(int login_id, int roleid)
        {
            LayoutWinDto dto = new LayoutWinDto()
            {
                TopBar = new List<MenuBar>()
            };
            BizResult<LayoutWinDto> result = new BizResult<LayoutWinDto>();
            BizResult<IList<FW_U_Rights>> bizresult = this.RoleBiz.GetRights(login_id, roleid,false);
            if (result.Success = bizresult.Success)
            {
                IList<FW_U_Rights> rights = bizresult.Data;
                dto.UserInfo = this.AccountBiz.GetUserInfo(login_id);
                dto.LoginInfo = this.AccountBiz.GetLoginInfo(login_id);
                dto.ShortCuts = new ShortCutCollection(53, 27);
                IEnumerable<FW_U_Rights> short_cuts = rights.Where(p => p.is_shortcut);
                foreach (FW_U_Rights item in short_cuts)
                {
                    dto.ShortCuts.Add(new ShortCut
                    {
                        Id = item.id.ToString(),
                        IsWin = item.is_win,
                        Name = Tools.GetResourceString(item.right_name_rs_key, item.right_name),
                        Ico = string.IsNullOrEmpty(item.ico) ? "none.jpg" : item.ico,
                        Url = item.limit_url,
                        WinSize = (WinSizeEnum)item.win_size,
                        Win_Height = item.win_height,
                        Win_Width = item.win_width,
                        WinTitle = Tools.GetResourceString(item.title_rs_key, item.title)
                    });
                }


                IEnumerable<FW_U_Rights> _rights = rights.Where(p => p.is_menu);
                IEnumerable<FW_U_Rights> top_meun_list = _rights.Where(p => p.parent_id.Equals(0));
                IEnumerable<FW_U_Rights> top_meun_chrildren = _rights.Where(p => !p.parent_id.Equals(0));
                foreach (FW_U_Rights item in top_meun_list)
                {
                    dto.TopBar.Add(new MenuBar(item.right_name)
                    {
                        Children = GetGroups(item.id, top_meun_chrildren)
                    });
                }
 
                result.Data = dto;
            }
            return result;
        }
    }

   
    /// <summary>
    /// 私有成员
    /// </summary>
    public partial class LayoutBiz
    {
        /// <summary>
        /// 获取desktop布局方式使用的菜单数据
        /// </summary>
        /// <param name="rights"></param>
        /// <returns></returns>
        private TopBar GetTopBar(IList<FW_U_Rights> rights)
        {
            TopBar topBar = new TopBar
            {

                TopMenus = new List<TopMenu>()
            };
            if (rights != null && rights.Count() > 0)
            {
                IEnumerable<FW_U_Rights> _rights = rights.Where(p => p.is_menu);
                IEnumerable<FW_U_Rights> top_meun_list = _rights.Where(p => p.parent_id.Equals(0));
                IEnumerable<FW_U_Rights> top_meun_chrildren = _rights.Where(p => !p.parent_id.Equals(0));
                foreach (FW_U_Rights item in top_meun_list)
                {
                    string topname = Tools.GetResourceString(item.right_name_rs_key) ?? item.right_name;
                    TopMenu topmenu = new TopMenu(topname)
                    {
                        Id = item.id.ToString(),
                        Url = item.limit_url,
                        IsWin = item.is_win,
                        WinSize = (WinSizeEnum)item.win_size,
                        Sort = item.sort,
                        Children = CreateMenu(item, top_meun_chrildren),
                        Win_Height = item.win_height,
                        Win_Width = item.win_width,
                        WinTitle = Tools.GetResourceString(item.title_rs_key)
                    };
                    topBar.TopMenus.Add(topmenu);
                }
            }
            return topBar;
        }
        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <param name="child">当前节点</param>
        /// <param name="children">子节点</param>
        /// <returns></returns>
        private IList<MenuGroup> CreateMenu(FW_U_Rights child, IEnumerable<FW_U_Rights> children)
        {
            IList<MenuGroup> group = new List<MenuGroup>();
            IEnumerable<FW_U_Rights> list = children.Where(p => p.parent_id.Equals(child.id));
            IEnumerable<FW_U_Rights> unlist = children.Where(p => !p.parent_id.Equals(child.id));
            foreach (FW_U_Rights right in list)
            {
                Menu menu = new Menu
                {
                    Id = right.id.ToString(),
                    Name = Tools.GetResourceString(right.right_name_rs_key, right.right_name),
                    Attributes = null,
                    Url = right.limit_url,
                    Children = null,
                    Sort = right.sort,
                    WinSize = (WinSizeEnum)right.win_size,
                    IsWin = right.is_win,
                    Win_Height = right.win_height,
                    Win_Width = right.win_width,
                    WinTitle = Tools.GetResourceString(right.title_rs_key)
                };
                IEnumerable<FW_U_Rights> children2 = unlist.Where(p => p.parent_id.Equals(right.id));
                if (children2 != null && children2.Count() > 0)
                {
                    menu.Children = CreateMenu(right, unlist);
                }

                MenuGroup mgroup = group.FirstOrDefault(p => p.GroupNum.Equals(right.group_num));
                if (mgroup == null)
                {
                    mgroup = new MenuGroup();
                    mgroup.GroupNum = right.group_num;
                    group.Add(mgroup);
                }

                if (right.dev_key == Constans.DEVELOP_KEY_FOR_LANGUAGE)
                {
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
                        if (key.Equals(this.CurrentLang))
                        {
                            lang_menu.Attributes = new { @class = "selected" };
                        }
                        if (menu.Children == null) menu.Children = new List<MenuGroup>();
                        MenuGroup _mgroup = new MenuGroup();
                        _mgroup.Add(lang_menu);
                        menu.Children.Add(_mgroup);
                    }
                }

                mgroup.Add(menu);

            }
            return group;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="login_id"></param>
        /// <returns></returns>
        private IList<FW_U_Shortcut> GetShortCuts(int login_id)
        {
            return this.ProDb.GetEntities<FW_U_Shortcut>("select id, shortcut_name,shortcut_name_rs_key, url, login_id, create_time,is_win,win_size,ico,win_width,win_height from FW_U_Shortcut where login_id=@uid order by sort asc",
                        new DbParam("@uid", login_id));
        }
        /// <summary>
        /// 获取菜单分组
        /// </summary>
        /// <param name="topid">当前顶级编号</param>
        /// <param name="top_meun_chrildren">子级权限</param>
        /// <returns></returns>
        private IList<MGroup> GetGroups(int topid, IEnumerable<FW_U_Rights> top_meun_chrildren)
        {
            IList<MGroup> groups = new List<MGroup>();
            IEnumerable<FW_U_Rights> gps = top_meun_chrildren.Where(p => p.parent_id.Equals(topid));

            foreach (FW_U_Rights item in gps)
            {
                groups.Add(new MGroup(item.right_name)
                {
                     Ico = item.ico,
                    Children = GetMenus(item.id, top_meun_chrildren)

                });
            }
            return groups;
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="parentid">父级菜单编号</param>
        /// <param name="top_meun_chrildren">子级权限</param>
        /// <returns></returns>
        private IList<Menu> GetMenus(int parentid, IEnumerable<FW_U_Rights> top_meun_chrildren)
        {
            IList<Menu> menus = new List<Menu>();
            IEnumerable<FW_U_Rights> children = top_meun_chrildren.Where(p => p.parent_id.Equals(parentid));
            foreach (FW_U_Rights item in children)
            {
                menus.Add(new Menu
                {
                    Id = item.id.ToString(),
                    Name = Tools.GetResourceString(item.right_name_rs_key, item.right_name),
                    Attributes = null,
                    Url = item.limit_url,
                    Children = CreateMenu(item, top_meun_chrildren),
                    Sort = item.sort,
                    WinSize = (WinSizeEnum)item.win_size,
                    IsWin = item.is_win,
                    Win_Height = item.win_height,
                    Win_Width = item.win_width,
                    WinTitle = Tools.GetResourceString(item.title_rs_key),
                    Ico = item.ico
                });
            }
            return menus;
        }
    }
}
