using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ez.Controllers.Library
{
    /// <summary>
    /// 产品授权
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LicenseAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            #if !DEBUG
            //filterContext.HttpContext.Response.Redirect("/buy.html",false);
            #endif

        }
    }
}
