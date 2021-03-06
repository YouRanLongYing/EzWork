﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Ez.Core;
namespace Ez.UI.HtmlExtends.Controls
{
    public class HtmlSimpleEditor : BaseHtmlControl
    {
        public HtmlSimpleEditor()
        {

        }

        public override string CreateHtmlString()
        {
            MvcHtmlString htmlstring = this.HtmlHelper.TextArea(
                this.MetaData.PropName, this.MetaData.PropValue.ToSafeString(), MetaData.RoutValueDic);
            return htmlstring.FormItemWraperForTextBox(this.MetaData.PropName, this.MetaData.LabelName).ToHtmlString();
        }
    }
}
