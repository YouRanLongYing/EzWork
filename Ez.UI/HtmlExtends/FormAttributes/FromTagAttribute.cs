using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace Ez.UI.HtmlExtends.FormAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FromTagAttribute : Attribute
    {
        private bool isAsyncSubmit = true;

        public bool IsAsyncSubmit
        {
            get { return isAsyncSubmit; }
            set { isAsyncSubmit = value; }
        }

        public string Subject { set; get; }
        public string ActionName { set; get; }
        public string ControllerName { set; get; }
        public RouteValueDictionary RouteValues { set; get; }
        public FormMethod Method { set; get; }
        public FromTagAttribute(string actionName, string controllerName)
            : this(actionName, controllerName, null, FormMethod.Post)
        {


        }
        public FromTagAttribute(string actionName, string controllerName, RouteValueDictionary routeValues, FormMethod method)
        {
            this.ActionName = actionName;
            this.ControllerName = controllerName;
            this.RouteValues = routeValues;
            this.Method = method;
        }
    }
}
