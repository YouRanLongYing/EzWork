using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Spring.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using UBIQ.Framework.UI;
using UBIQ.Framework.Web;
using UBIQ.Framework.Lang;

namespace $safeprojectname$
{

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : SpringMvcApplication//UBIQ.Site.Framework.MvcApplication ////System.Web.HttpApplication//
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
                //Codeing...
            filters.Add(new CultureInfoAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            //Codeing...
        }

        protected void Application_Start()
        {
            FrameworkApplication.Instance.AppStart(new LanguageAssemblyProvider
            {
                AssemblyName = "$saferootprojectname$.Resource.Language",
                AssemblyType = typeof($saferootprojectname$.Resource.Language)
            });

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RouteTable.Routes.RouteExistingFiles = false;
        }
    }
}