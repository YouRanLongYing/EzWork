using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ez.Core;
using Ez.Config;

namespace Ez.UI.Attributes
{
    public class LayoutAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }

        private const string DESKTOP_ACTION_NAME = "desktop";
        private string TRADITION_ACTION_NAME = UIConfig.Model.LayoutAction;//"tradition";
        private const string LAYOUT_CONTROLLER_NAME = "window";
        //系统UI布局控制
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            /*设置桌面布局方式需要的样式文件的所在目录*/
            string action = filterContext.ActionDescriptor.ActionName;
            string controllername = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (DESKTOP_ACTION_NAME.Equals(action.ToLower()) && LAYOUT_CONTROLLER_NAME.Equals(controllername.ToLower()))
            {
                filterContext.HttpContext.Session[Constans.SYS_LAYOUT_STYLE_KEY] = DESKTOP_ACTION_NAME;
            }
            else if (TRADITION_ACTION_NAME.Equals(action.ToLower()) && LAYOUT_CONTROLLER_NAME.Equals(controllername.ToLower()))
            {
                filterContext.HttpContext.Session[Constans.SYS_LAYOUT_STYLE_KEY] = TRADITION_ACTION_NAME;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.Result is JsResult)
            {
                //异步请求
                filterContext.HttpContext.Response.Write(filterContext.Result);
            }
            else if (filterContext.Result is ViewResult)
            {
                ViewResult result = (filterContext.Result as ViewResult);
                result.ViewData["LayoutAction"] = filterContext.HttpContext.Session[Constans.SYS_LAYOUT_STYLE_KEY] ?? TRADITION_ACTION_NAME;
                result.ViewData["UIStyle"] = UIConfig.Model.UIStyle ?? "default";
                result.ViewData["RequiredMenuFrom"] = filterContext.HttpContext.Request.RawUrl;
                result.ViewData["SystemName"] = UIConfig.Model.SystemName;
                
            }
        }
    }
}
