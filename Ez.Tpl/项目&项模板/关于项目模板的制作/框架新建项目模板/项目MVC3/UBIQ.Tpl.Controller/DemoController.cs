using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Framework.Controllers.Library;
using $saferootprojectname$.IBiz;
using System.Web.Mvc;

namespace $safeprojectname$
{
    /// <summary>
    /// clrversion:$clrversion$
    /// Guid:$guid1$
    /// itemname:$itemname$
    /// machinename:$machinename$
    /// projectname:$projectname$
    /// registeredorganization:$registeredorganization$
    /// safeprojectname:$safeprojectname$
    /// userdomain:$userdomain$
    /// username:$username$
    /// webnamespace:$webnamespace$
    /// time:$time$
    /// </summary>
    public class DemoController: DefaultController<IDemoBiz>
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
