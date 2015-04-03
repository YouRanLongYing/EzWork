using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;
using UBIQ.Core.Framework;
using UBIQ.Core.Framework.Interceptor;
using UBIQ.Helper.Framework;
using UBIQ.Dtos.Framework;
using UBIQ.UI.Framework;
namespace UBIQ.Controllers.Framework.Lib
{
    public abstract class AjaxControllerHandler : DefaultController, IAjaxServiceHandler
    {
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    try
        //    {
        //        string methondName = filterContext.ActionDescriptor.ActionName;
        //        MethodInfo action = this.GetType().GetMethod(methondName);
        //        if (action != null)
        //        {
        //            string commond = Request["commond"];
        //            JsResult result = null;
        //            try
        //            {
        //                result = action.Invoke(this, new object[] { commond }) as JsResult;
        //            }
        //            catch (Exception exp)
        //            {
        //                filterContext.Result = GetActionResult(new JsResult("", false, "", "方法执行时发生异常," + exp.Message, ""));
        //            }

        //            if (result != null)
        //            {
        //                filterContext.Result = GetActionResult(result);
        //            }
        //            else
        //            {
        //                throw new NullReferenceException("方法'" + methondName + "'的返回值为空！");
        //            }
        //        }
        //        else
        //        {
        //            throw new MissingMethodException("调用的方法'" + methondName + "'不存在！");
        //        }

        //        base.OnActionExecuting(filterContext);
        //    }
        //    catch (Exception exp)
        //    {
        //        filterContext.Result = GetActionResult(new JsResult("", false, "", exp.Message, ""));
        //    }
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                throw new InvalidOperationException("此处只接受ajax请求！");
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsResult)
            {
                filterContext.Result = GetActionResult(filterContext.Result as JsResult);
            }
            else
            {
                throw new NullReferenceException("方法'" + filterContext.ActionDescriptor.ActionName + "'的返回值类型错误，请检查方法或网络问题！");
            }
            base.OnActionExecuted(filterContext);
        }

        private ActionResult GetActionResult(JsResult result)
        {
            object data = null;
            ActionResult aResult = null;
            if (result.Data != null)
            {
                TypeCode dataType = Type.GetTypeCode(result.Data.GetType());
                data = result.Data;
                if (dataType == TypeCode.String)
                {
                    data = data.ToString().Replace("\r", "").Replace("\n", "");
                }
            }
            else
            {
                data = "{}";
            }
            aResult = Json(new
            {
                success = result.Success,
                data = data,
                message = result.Message,
                errorMsg = result.ErrorMessage,
                waringMsg = result.WraingMessage,
                redirect = result.Redirect,
                xdialogid = result.XdialogId,
                isCloseXdialog = result.CloseXdialog,
                redirectToBroswerTab = result.RedirctToBroswerTab
            }, JsonRequestBehavior.AllowGet);

            return aResult;
        }

    }

}
