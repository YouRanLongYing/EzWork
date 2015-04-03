using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Ez.UI.HtmlExtend;
using Ez.Core;
namespace Ez.UI.HtmlExtends.Controls
{
    public class HtmlCheckBox : BaseHtmlControl
    {
        public HtmlCheckBox()
        {
            
        }
        private MvcHtmlString ChekcBoxOne(HtmlHelper htmlHelper,string hiddenName, string name, PropAgentItem item, RouteValueDictionary rvdic)
        {
            if (!string.IsNullOrEmpty(hiddenName))
            {
                StringBuilder innerhtml = new StringBuilder();

                string perid = hiddenName.Replace(".","_").Replace("[", "_").Replace("]", "_");
                innerhtml.Append("<label class=\"am-radio-inline\">");
                innerhtml.AppendFormat("<input id=\"{0}\" name=\"{1}\" type=\"checkbox\" value=\"{2}\">", name, name, item.Value);
                innerhtml.AppendFormat("<input id=\"{0}Text\" name=\"{1}key\" type=\"hidden\" value=\"{2}\">", perid, hiddenName, item.Text);
                innerhtml.AppendFormat("<input id=\"{0}Value\" name=\"{1}Value\" type=\"hidden\" value=\"{2}\">", perid, hiddenName, item.Value);
                //innerhtml.AppendFormat("<input id=\"{0}.Selected\" name=\"{1}.Selected\" type=\"hidden\" value=\"{2}\">",perid, hiddenName, item.Selected);
                innerhtml.AppendFormat("<input id=\"{0}PropName\" name=\"{1}PropName\" type=\"hidden\" value=\"{2}\">", perid, hiddenName, "");
                innerhtml.Append(item.Text);
                innerhtml.Append("</label>");
                return MvcHtmlString.Create(innerhtml.ToString());
            }
            else
            {
                return MvcHtmlString.Create(string.Format("<label class=\"am-radio-inline\"><input id=\"{0}\" name=\"{1}\" type=\"checkbox\" value=\"{2}\">{3}</label>", name.Replace("[", "_").Replace("]", "_"), name, item.Value, item.Text));
            }
        }
        public override string CreateHtmlString()
        {
            if (this.MetaData.PropValue is PropAgent)
            {
                StringBuilder append = new StringBuilder();
                PropAgent pagent = this.MetaData.PropValue as PropAgent;
                for(int i=0;i<pagent.Length;i++)
                {
                    PropAgentItem item = pagent[i];
                    RouteValueDictionary rvd = this.MetaData.RoutValueDic ?? new RouteValueDictionary();
                    append.Append(ChekcBoxOne(this.HtmlHelper, string.Format("{0}[{1}].", this.MetaData.PropName,i), pagent.PropName, item, rvd));
                }
                return MvcHtmlString.Create(append.ToString()).FormItemWraperForRadioOrCheckbox(this.MetaData.LabelName).ToHtmlString();

            }
            else
            {
                return ChekcBoxOne(this.HtmlHelper, "", this.MetaData.PropName, new PropAgentItem(this.MetaData.LabelName, this.MetaData.PropValue.ToSafeString()), this.MetaData.RoutValueDic)
                    .FormItemWraperForRadioOrCheckbox("").ToHtmlString();
            }
        }
    }
}
