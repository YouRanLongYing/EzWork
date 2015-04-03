using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using UBIQ.Cache.Framework;
using UBIQ.Dtos.Framework;
using System.Web;
using System.Reflection;
using UBIQ.IBiz.Framework;
using UBIQ.Core.Framework;
using UBIQ.UI.Framework;
using UBIQ.Dtos.Framework.Lib;
using UBIQ.Config.Framework;

namespace UBIQ.Controllers.Framework.Lib
{
    /// <summary>
    /// 认证的内容
    /// </summary>
    public enum AuthenticationEnum
    {
        /// <summary>
        /// 登录身份验证
        /// </summary>
        Identity,
        /// <summary>
        /// 角色认证
        /// </summary>
        Role,
        /// <summary>
        /// 权限认证
        /// </summary>
        Right
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationAttribute : AuthorizeAttribute
    {
        private AuthenticationEnum authEnum;
        private bool identityFailToRedirectToLogin = true;
        /// <summary>
        /// 用于判断在登录身份认证失败的情况下是否跳转到登录页面否则跳转到注册页面
        /// </summary>
        public bool IdentityFailToRedirectToLogin
        {
            get { return identityFailToRedirectToLogin; }
            set { identityFailToRedirectToLogin = value; }
        }
        public AuthenticationAttribute()
        {
            this.authEnum = AuthenticationEnum.Identity;
        }
        /// <summary>
        /// 用于认证角色或权限的
        /// </summary>
        /// <param name="authEnum">认证内容</param>
        public AuthenticationAttribute(AuthenticationEnum authEnum)
        {
            this.authEnum = authEnum;
        }
        /// <summary>
        /// 执行认证
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            switch (this.authEnum)
            {
                case AuthenticationEnum.Identity: Identity(filterContext); break;
                case AuthenticationEnum.Role: Role(filterContext); break;
                case AuthenticationEnum.Right: Right(filterContext); break;
            }
            //base.OnAuthorization(filterContext);
        }

        private bool IsFromLoginPage
        {
            get
            {
                string currenturl = HttpContext.Current.Request.Url.ToString();
                int index = currenturl.IndexOf("?");
                if (index > 0)
                {
                    currenturl = currenturl.Substring(0, index);
                }
                return currenturl.ToLower().EndsWith("login") || currenturl.ToLower().EndsWith("loginforajax");
            }
        }
        private bool IsFromAjaxPager {

            get {
                var xgrid = HttpContext.Current.Request["xgrid"];
                return xgrid == "xgrid";
            }
        }
        /// <summary>
        /// 登录身份认证
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using System.Web.Mvc.AuthorizeAttribute.</param>
        private void Identity(AuthorizationContext filterContext)
        {
            bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
            LoginInfoDto dto = null;
            object obj = SessionProxy.Instance.GetLoginInfo();
            if (obj != null)
            {
                dto = SessionProxy.Instance.GetLoginInfo() as LoginInfoDto;
            }
            var unlogined = dto == null || dto.id <= 0;
            string loginpage = UIConfig.Model.Login ?? "/Window/Login";
            if (unlogined)
            {
                if (isAjaxRequest)//异步请求
                {
                    if (this.IsFromAjaxPager)
                    {
                        filterContext.RequestContext.HttpContext.Response.Write(PageDto<object>.NoRightProcess());
                        filterContext.RequestContext.HttpContext.Response.End();
                    }
                    else if (!this.IsFromLoginPage)
                    {
                        filterContext.RequestContext.HttpContext.Response.Write(new JsResult(null, true, loginpage) { JsonRequestBehavior = JsonRequestBehavior.AllowGet });
                        filterContext.RequestContext.HttpContext.Response.End();
                    }
                }
                else if (!this.IsFromLoginPage)//非异步请求、可能是 HttpRequest的GET或POST
                {
                        if (this.IdentityFailToRedirectToLogin)
                        {
                            filterContext.Result = new RedirectResult(loginpage+"?returnUrl=" + HttpContext.Current.Request.Url);
                        }
                        else
                        {
                            filterContext.Result = new RedirectResult(loginpage+"?returnUrl=" + HttpContext.Current.Request.Url);
                        }
                }
            }
            else if (!isAjaxRequest)
            {
                /*如果存在导向的定义则按照导向执行跳转*/
                string redirect = HttpContext.Current.Request["returnUrl"];
                var b = redirect != null || this.IsFromLoginPage;
                if (!b)
                {
                    /*向当前请求的控制注入已登录的用户身份信息*/
                    PropertyInfo property = filterContext.Controller.GetType().GetProperty("CurrentUser");
                    if (property != null)
                    {
                        property.SetValue(filterContext.Controller, dto, null);
                    }
                }
                else if (redirect != null)
                {
                    HttpContext.Current.Response.Redirect(redirect, true);
                }
                else if (this.IsFromLoginPage)
                {
                    filterContext.Result = new RedirectResult("/");
                }
            }
        }
        private void Role(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            BizResult<int> resule = AuthorizationBizInstance.LawfulRole(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName);
            if (resule.Success)
            {

            }
            else
            {
                throw new Exception("无权限");
            }
        }
        private void Right(AuthorizationContext filterContext)
        {

        }
        private static IAuthorizationBiz authorizationBizInstance;
        internal static IAuthorizationBiz AuthorizationBizInstance
        {
            get
            {
                if (Object.ReferenceEquals(authorizationBizInstance, null))
                {
                    authorizationBizInstance = Utils.GetSpringObject<IAuthorizationBiz>("AuthorizationBiz");
                }
                return authorizationBizInstance;
            }

        }
    }
}
