using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Ez.Lang;
namespace Ez.UI.HtmlExtends.Controls
{
    public class HtmlUploadFile : BaseHtmlControl
    {
        public override string CreateHtmlString()
        {

            TagBuilder tag = new TagBuilder("a");
            tag.MergeAttributes(this.MetaData.RoutValueDic,true);
            tag.SetInnerText(this.MetaData.LabelName??EzLanguage.COM_Btn_Broswer);

            return MvcHtmlString.Create(tag.ToMvcHtmlString(TagRenderMode.Normal).ToHtmlString() + this.HtmlHelper.Hidden(
                this.MetaData.PropName, this.MetaData.PropValue, new {fileup="yes" }).ToHtmlString()).
                FormItemWraperForTextBox(this.MetaData.PropName, this.MetaData.LabelName).ToHtmlString();
        }
    }
}
