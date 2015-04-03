using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Controllers.Library;
using System.Web.Mvc;

namespace Ez.Controllers
{
    public class Template1Controller:BaseController
    {
        public ActionResult Tmp_Login()
        {
            return View();
        }
    }
}
