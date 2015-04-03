using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Framework.Controllers.Library;
using UBIQ.Tpl.IBiz;
using System.Web.Mvc;

namespace UBIQ.Tpl.Controller
{
    public class DemoController: DefaultController<IDemoBiz>
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
