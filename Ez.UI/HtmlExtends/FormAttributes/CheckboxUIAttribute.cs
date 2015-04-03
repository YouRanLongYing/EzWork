using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
namespace Ez.UI.HtmlExtends.FormAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CheckBoxUIAttribute : PropertyUIAttribute
    {

        public override  TagType UIType { get { return TagType.CheckBox; } }

        public override RouteValueDictionary GetAttributes()
        {
            return base.GetAttributes(); ;
        }
    }
}
