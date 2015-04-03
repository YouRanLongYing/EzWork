using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace Ez.UI.ViewExtend
{
    public abstract class CRouter
    {

        protected CRouter() { }

        /// <summary>
        /// 扩展路由表
        /// </summary>
        /// <param name="routes"></param>
        public static void MapRouterRules(RouteCollection routes)
        {
            string regexRule = "^[A-Za-z0-9]+$";
            string controllerRule = regexRule;
            string actionRule = regexRule;
            routes.MapRoute("index", "{controller}/{action}", new { action = "Index" }, new { controller = controllerRule });

            routes.MapRoute("p41","{path1}/{path2}/{path3}/{path4}/{controller}/{action}/{id}",
                new { controller = "", action = "", id = "" },
                new { controller = controllerRule, action = actionRule, path1 = regexRule, path2 = regexRule, path3 = regexRule, path4 = regexRule });
            routes.MapRoute("p4", "{path1}/{path2}/{path3}/{path4}/{controller}/{action}",
                new { controller = "", action = "" }, 
                new { controller = controllerRule, action = actionRule, path1 = regexRule, path2 = regexRule, path3 = regexRule, path4 = regexRule });
            routes.MapRoute("p31", "{path1}/{path2}/{path3}/{controller}/{action}/{id}",
                new { controller = "", action = "", id = "" },
                new { controller = controllerRule, action = actionRule, path1 = regexRule, path2 = regexRule, path3 = regexRule });
            routes.MapRoute("p3", "{path1}/{path2}/{path3}/{controller}/{action}",
                new { controller = "", action = "" },
                new { controller = controllerRule, action = actionRule, path1 = regexRule, path2 = regexRule, path3 = regexRule });
            routes.MapRoute("p21", "{path1}/{path2}/{controller}/{action}/{id}",
                new { controller = "", action = "", id = "" },
                new { controller = controllerRule, action = actionRule, path1 = regexRule, path2 = regexRule });
            routes.MapRoute("p2", "{path1}/{path2}/{controller}/{action}",
                new { controller = "", action = "" },
                new { controller = controllerRule, action = actionRule, path1 = regexRule, path2 = regexRule });
            routes.MapRoute("p11", "{path1}/{controller}/{action}/{id}",
                new { controller = "", action = "", id = "" },
                new { controller = controllerRule, action = actionRule, path1 = regexRule });
            routes.MapRoute("p0", "{path1}/{controller}/{action}",
                new { controller = "", action = "" },
                new { controller = controllerRule, action = actionRule, path1 = regexRule });

        }
    }
}
