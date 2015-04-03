using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.IBiz.Framework;
using System.Web.Mvc;
using UBIQ.Dtos.Framework;
using UBIQ.Controllers.Framework.Lib;
using UBIQ.Cache.Framework;
using UBIQ.UI.Framework;
using UBIQ.Resource.Framework;
using UBIQ.Dtos.Framework.Lib;
using UBIQ.Dtos.Framework.Models;

namespace UBIQ.Controllers.Framework
{
    public partial class TicketController : DefaultController
    {
        public ActionResult Index()
        {
            return View();
        }
    }

}
