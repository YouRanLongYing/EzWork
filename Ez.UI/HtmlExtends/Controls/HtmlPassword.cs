using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace Ez.UI.HtmlExtends.Controls
{
    public class HtmlPassword : BaseHtmlControl
    {
        public HtmlPassword()
        {
           
        }

        public override string CreateHtmlString()
        {
            MvcHtmlString htmlstring = this.HtmlHelper.Password(this.MetaData.PropName, this.MetaData.PropValue, this.MetaData.RoutValueDic);
            return htmlstring.FormItemWraperForTextBox(this.MetaData.PropName, this.MetaData.LabelName).ToHtmlString();
        }
    }
}
