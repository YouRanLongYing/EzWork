using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Ez.UI.HtmlExtend;
namespace Ez.UI.HtmlExtends.Controls
{
    public class HtmlRadio: BaseHtmlControl
    {
        public HtmlRadio()
        {

        }
        private MvcHtmlString RadioOne(HtmlHelper htmlHelper, string name, string text, object value,bool _checked, RouteValueDictionary rvdic)
        {
            return MvcHtmlString.Create("<label class=\"am-radio-inline\">" + htmlHelper.RadioButton(name, value, _checked, rvdic).ToHtmlString() + text + "</label>");
        }
        public override string CreateHtmlString()
        {
            if (this.MetaData.PropValue is PropAgent)
            {
                StringBuilder append = new StringBuilder();
                PropAgent pagent = this.MetaData.PropValue as PropAgent;

                foreach (PropAgentItem item in pagent)
                {
                    append.Append(RadioOne(this.HtmlHelper, pagent.PropName, item.Text, item.Value,item.Selected,this.MetaData.RoutValueDic));
                }
                return MvcHtmlString.Create(append.ToString()).FormItemWraperForRadioOrCheckbox(this.MetaData.LabelName).ToHtmlString();

            }
            else
            {
                return RadioOne(this.HtmlHelper, this.MetaData.PropName, this.MetaData.LabelName, this.MetaData.PropValue,false, this.MetaData.RoutValueDic)
                    .FormItemWraperForRadioOrCheckbox(this.MetaData.LabelName).ToHtmlString();
            }
        }
    }
}
