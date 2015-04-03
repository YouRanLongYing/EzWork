using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Ez.UI.HtmlExtend;
namespace Ez.UI.HtmlExtends.Controls
{
    public class HtmlSelect : BaseHtmlControl
    {
        public HtmlSelect()
        {

        }

        public override string CreateHtmlString()
        {
            if (this.MetaData.PropValue != null)
            {
                PropAgent pagent = this.MetaData.PropValue as PropAgent;
                if (pagent != null)
                {
                    IList<SelectListItem> items = new List<SelectListItem>();
                    foreach (PropAgentItem item in pagent)
                    {
                        items.Add(new SelectListItem
                        {
                            Selected = item.Selected,
                            Text = item.Text,
                            Value = item.Value
                        });
                    }
                    MvcHtmlString htmlstring = this.HtmlHelper.DropDownList(
                        pagent.PropName, items, this.MetaData.RoutValueDic);
                    return htmlstring.FormItemWraperForTextBox(this.MetaData.PropName,
                        this.MetaData.LabelName).ToHtmlString();
                }
                else
                    return "请设置属性代理，使用请见文档。";
            }
            else
                return "";
        }
    }
}
