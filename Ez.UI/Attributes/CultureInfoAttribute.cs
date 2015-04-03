using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using Ez.Config;
using Ez.Core;


namespace Ez.UI
{
    /// <summary>
    /// 系统语言控制
    /// </summary>
    public class CultureInfoAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string cultureCode = System.Web.HttpContext.Current.Session[Constans.SYS_LANAGUE_KEY] as string;
            if (cultureCode == null)
            {
                System.Web.HttpContext.Current.Session[Constans.SYS_LANAGUE_KEY] = cultureCode = UIConfig.Model.Language;
            }

            try
            {
                CultureInfo culture = new CultureInfo(cultureCode);
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch (Exception exp)
            {
                throw new Exception("系统语言加载失败！" + exp.Message);
            }
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
