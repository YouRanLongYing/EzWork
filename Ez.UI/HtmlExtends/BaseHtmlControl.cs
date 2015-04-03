using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ez.UI.HtmlExtends
{
    public abstract class BaseHtmlControl
    {
        protected PropUIMetadata MetaData { set; get; }
        protected HtmlHelper HtmlHelper { set; get; }
        public void SetParameter(PropUIMetadata metaData, HtmlHelper htmlHelper)
        {
            this.MetaData = metaData;
            this.HtmlHelper = htmlHelper;
        }
        public abstract string CreateHtmlString();
    }
}
