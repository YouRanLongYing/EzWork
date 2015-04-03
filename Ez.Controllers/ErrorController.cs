using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ez.Dtos;
using Ez.UI;

namespace Ez.Controllers
{
    public class ErrorController:Controller
    {
        public ActionResult Index(string rawUrl,bool debug)
        {
            Exception exception = debug?this.RouteData.Values["error"] as Exception:null;
            return View(new ErrorDto(exception, rawUrl, debug));
        }
         [AsyncAction]
        public JsResult AsyncError(string rawUrl, bool debug)
        {
            Response.ClearContent();
            Exception exception = debug ? this.RouteData.Values["error"] as Exception : null;
            return new JsResult("", false, "抱歉请求发生错误！" + (exception == null ? "" : exception.StackTrace), false);
        }
    }
}
