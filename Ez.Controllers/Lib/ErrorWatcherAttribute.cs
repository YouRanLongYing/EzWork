using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using System.Runtime.Remoting.Contexts;
using UBIQ.Core.Framework.Interceptor;

namespace UBIQ.Controllers.Framework
{
    public class ErrorWatcherAttribute : ActionFilterAttribute, IExceptionFilter
    {

        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            //filterContext.Result = new RedirectResult("/Error");//跳转至错误提示页面
            IController errorController = new ErrorController();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Index");
            routeData.Values.Add("error", filterContext.Exception);
            routeData.Values.Add("rawUrl",filterContext.HttpContext.Request.RawUrl);
            routeData.Values.Add("debug", Log4NetManager.DefaultLogger.IsDebugEnabled);
            errorController.Execute(new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData));
        }
    }
}
