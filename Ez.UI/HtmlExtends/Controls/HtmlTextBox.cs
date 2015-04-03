using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
namespace Ez.UI.HtmlExtends.Controls
{
    public class HtmlTextBox : BaseHtmlControl
    {
        public HtmlTextBox()
        {
        }

        public override string CreateHtmlString()
        {
            MvcHtmlString htmlstring = this.HtmlHelper.TextBox(this.MetaData.PropName, this.MetaData.PropValue, this.MetaData.RoutValueDic);
            return htmlstring.FormItemWraperForTextBox(this.MetaData.PropName, this.MetaData.LabelName).ToHtmlString();
        }
    }
}
