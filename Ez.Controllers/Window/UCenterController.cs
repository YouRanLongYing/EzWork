using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;
using System.Web.Mvc;
using Ez.Dtos;
using Ez.Controllers.Library;
using Ez.Cache;
using Ez.UI;
using Ez.Lang;
using Ez.Dtos.Library;
using Ez.Dtos.Entities;

namespace Ez.Controllers
{
    /// <summary>
    /// 用户中心控制器
    /// created by kongjing
    /// </summary>
    [Authentication(AuthenticationEnum.Identity)]
    public partial class UCenterController : DefaultController<IAccountBiz>
    {
        /// <summary>
        /// 用户信息编辑页面初始化
        ///  create by kongjing
        /// </summary>
        /// <returns></returns>
        public ActionResult EditMyInfo()
        {
            var dto = this.DefaultBiz.GetUserInfo(this.CurrentUser.id) ?? new UserInfoDto();
            if (dto.login_id <= 0)
                dto.login_id = this.CurrentUser.id;
            return View(dto);
        }
        /// <summary>
        /// 提交编辑
        /// created by kongjing
        /// </summary>
        [HttpPost]
        [AsyncAction]
        public JsResult EditMyInfoAsync(UserInfoDto dto)
        {

            BizResult<UserInfoDto> result = this.DefaultBiz.SetUserInfo(dto);
            if (dto.id > 0)
            {
                return result.Success ? new JsResult(null, true, EzLanguage.COM_Msg_EditSuccess, true) : new JsResult(null, false, EzLanguage.COM_Msg_EditFail, false);
            }
            else
            {
                return result.Success ? new JsResult(null, true, EzLanguage.COM_Msg_SaveSuccess, true) : new JsResult(null, false, EzLanguage.COM_Msg_SaveFail, false);
            }
        }

        /// <summary>
        /// 创建一个用户页面初始化
        /// created by kongjing
        /// </summary>
        public ActionResult CreateUser()
        {
            return View(new LoginInfoDto());
        }

        /// <summary>
        /// 创建用户
        /// create by kongjing
        /// </summary>
        [HttpPost]
        [AsyncAction]
        public JsResult AsyncCreateUser(LoginInfoDto dto)
        {
            BizResult<LoginInfoDto> result = this.DefaultBiz.CreateUser(dto);
            return result.Success ? new JsResult(new { uid = result.Data.id }, true, EzLanguage.COM_Msg_SaveSuccess, true) : new JsResult(null, false, EzLanguage.COM_Msg_SaveFail, false);
        }
    }

    public partial class UCenterController
    {
        /// <summary>
        /// 角色列表
        /// created by kongjing
        /// </summary>
        public ActionResult RoleView()
        {
            return View();
        }

        /// <summary>
        /// 角色列表
        ///  created by kongjing
        /// </summary>
        [AsyncAction]
        public JsResult AsyncRoleView()
        {
            PageDto<FW_U_Roles> dto = new PageDto<FW_U_Roles>();
            BizResult<PageDto<FW_U_Roles>> exereturn = this.DefaultBiz.RoleBiz.GetRoleList(dto);
            if (exereturn.Success)
                return exereturn.Data.AsJsResult();
            else
                return new JsResult(false, new {Error="QueryError"}, "");
        }
    }
}
