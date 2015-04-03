using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ez.Lang;
using System.Web;
using System.Web.Script.Serialization;
namespace Ez.UI
{
    /// <summary>
    /// 异步相应结果
    /// created by kongjing
    /// </summary>
    public class JsResult:JsonResult
    {
        private bool redirctToBroswerTab = false;
        public bool RedirctToBroswerTab
        {
            get { return redirctToBroswerTab; }
        }
        private bool closeXdialog = false;

        public bool CloseXdialog
        {
            get { return closeXdialog; }
        }
        /// <summary>
        /// 操作反馈
        /// </summary>
        /// <param name="data">响应的数据</param>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        /// <param name="closeXdialog">若要在提示后关闭窗口请设置为true</param>
        public JsResult(object data, bool success, string message,bool closeXdialog)
        {
            this.Data = data;
            this.success = success;
            this.message = message;
            this.closeXdialog = closeXdialog;
        }
        /// <summary>
        /// 消息反馈后跳转
        /// </summary>
        /// <param name="data">响应的数据</param>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        /// <param name="redirect">跳转页面</param>
        /// <param name="redirctToBroswerTab">若跳转到浏览器的tab显示请设置为true</param>
        /// <param name="closeXdialog">若要在提示后关闭窗口请设置为true,若设置为true则参数redirctToBroswerTab的设置将失效，默认被强制设置为true</param>
        public JsResult(object data, bool success, string message, string redirect, bool redirctToBroswerTab, bool closeXdialog)
        {
            this.Data = data;
            this.success = success;
            this.message = message;
            this.redirect = redirect;
            this.redirctToBroswerTab = redirctToBroswerTab;
            if (closeXdialog) this.redirctToBroswerTab = true;
            this.closeXdialog = closeXdialog;
        }
        /// <summary>
        /// 直接跳转
        /// </summary>
        /// <param name="data">响应的数据</param>
        /// <param name="success">是否成功</param>
        /// <param name="redirect">跳转页面</param>
        public JsResult(object data, bool success, string redirect)
        {
            this.Data = data;
            this.success = success;
            this.redirect = redirect;
            this.redirctToBroswerTab = true;
        }
        /// <summary>
        /// 只做反馈
        /// </summary>
        /// <param name="data">响应的数据</param>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        public JsResult(bool success,object data, string message)
        {
            this.Data = data;
            this.success = success;
            this.message = message;
            this.closeXdialog = false;
            this.redirect = "";
        }
        private bool isSimpleJson = false;
        /// <summary>
        /// 不附加json内容 For Grid
        /// </summary>
        /// <param name="data"></param>
        public JsResult(object data)
        {
            isSimpleJson = true;
            this.Data = data;
            this.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
        }
        private bool success;
        public bool Success
        {
            get { return success; }
        }

        //private object data;
        //public object Data
        //{
        //    get
        //    {
        //        return data;
        //    }
        //}
        private string redirect;

        public string Redirect
        {
            get { return redirect; }
            set { redirect = value; }
        }
        private string message="";
        public string Message
        {
            get { return message; }
        }

        private string errorMessage="";
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        private string wraingMessage="";
        public string WraingMessage
        {
            get { return wraingMessage; }
            set { wraingMessage = value; }
        }
        public string XdialogId {

            get {
                return HttpContext.Current.Request["xdialogid"];
            }
        }
        //public JsonRequestBehavior JsonRequestBehavior { set; get; }
        //public string ContentType { set; get; }
        //public Encoding ContentEncoding { set; get; }
        public override string ToString()
        {
            #region
            //object data = this.Data;
            //if (data != null)
            //{
            //    TypeCode dataType = Type.GetTypeCode(this.Data.GetType());
            //    data = this.Data;
            //    if (dataType == TypeCode.String)
            //    {
            //        data = data.ToString().Replace("\r", "").Replace("\n", "");
            //    }
            //    else if (dataType == TypeCode.Object)
            //    {
            //        data = data.ToString().Replace("{", "{\"").Replace("=", "\":\"").Replace(",", "\",\"").Replace("}", "\"}");
            //    }
            //}
            //else
            //{
            //    data = "{}";
            //}

            //return string.Format("\"success\":{0},\"data\":{1},\"message\":\"{2}\",\"errorMsg\":\"{3}\",\"waringMsg\":\"{4}\",\"redirect\":\"{5}\",\"xdialogid\":\"{6}\",\"isCloseXdialog\":{7},\"redirectToBroswerTab\":{8}",
            //    this.success ? "true" : "false",
            //    data ?? "{}",
            //    this.message,
            //    this.errorMessage,
            //    this.wraingMessage,
            //    this.redirect,
            //    this.XdialogId,
            //    this.closeXdialog?"true":"false",
            //    this.redirctToBroswerTab?"true":"false");
            #endregion
            object data = null;
            if (this.Data != null)
            {
                TypeCode dataType = Type.GetTypeCode(this.Data.GetType());
                data = this.Data;
                if (dataType == TypeCode.String)
                {
                    data = data.ToString().Replace("\r", "").Replace("\n", "");
                }
            }
            else
            {
                data ="{}";
            }
            this.Data = data;
            HttpContext currentContext = HttpContext.Current;
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(currentContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("不允许Get方式请求！");
            }
            currentContext.Response.ContentType = "application/json";
            HttpResponse response = currentContext.Response;
            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            if (!this.isSimpleJson)
            {
                return serializer.Serialize(new
                {
                    success = this.Success,
                    data = this.Data,
                    message = this.Message,
                    errorMsg = this.ErrorMessage,
                    waringMsg = this.WraingMessage,
                    redirect = this.Redirect,
                    xdialogid = this.XdialogId,
                    isCloseXdialog = this.CloseXdialog,
                    redirectToBroswerTab = this.RedirctToBroswerTab
                });
            }
            else
                return serializer.Serialize(this.Data);

             
        }

        public override void ExecuteResult(ControllerContext context)
        {
           
        }
    }
}
