using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ez.UI
{
    /// <summary>
    /// 用于标记只能为异步请求action的特性
    /// created by kongjing
    /// </summary>
    public sealed class AsyncActionAttribute : ActionMethodSelectorAttribute
    {
        public AsyncActionAttribute()
        { 
          
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            if(controllerContext.HttpContext.Request.IsAjaxRequest())
            {
                if (!typeof(JsResult).Equals(methodInfo.ReturnType))
                {
                    throw new Exception("此action的返回值类型只能是" + typeof(JsResult)+"类型！");
                }
            }
            else
            {
                controllerContext.RequestContext.HttpContext.Response.StatusCode = 404;
                controllerContext.RequestContext.HttpContext.Response.StatusDescription = "请求地址不存在！";
                throw new Exception("请求地址不存在！");
            }
            return controllerContext.HttpContext.Request.IsAjaxRequest() && typeof(JsResult).Equals(methodInfo.ReturnType);
        }
    }
}
