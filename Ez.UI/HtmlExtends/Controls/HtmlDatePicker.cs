using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Ez.Core;
using Ez.UI.Validations;
namespace Ez.UI.HtmlExtends.Controls
{
    public class HtmlDatePicker : BaseHtmlControl
    {
        public HtmlDatePicker()
        {
            
        }

        public override string CreateHtmlString()
        {
            string formater = "yyyy-MM-dd HH:mm:ss";
            if (this.MetaData.PropInfo!=null&&Type.GetTypeCode(this.MetaData.PropInfo.PropertyType) == TypeCode.DateTime)
            {
                object[] obj = this.MetaData.PropInfo.GetCustomAttributes(typeof(InputTypeAttribute),false);
                if (obj != null && obj.Length>0)
                {
                    InputTypeAttribute type = obj[0] as InputTypeAttribute;
                    if (type != null && type.Dtype == Validations.InputType.DateTime)
                    {
                        switch (type.DateFormat)
                        {
                            case DateTimeFormat.HH_MM_SS: formater = "HH:mm:ss"; break;
                            case DateTimeFormat.YYYY_MM_DD: formater = "yyyy-MM-dd"; break;
                            case DateTimeFormat.YYYY_MM_DD__HH_MM_SS: formater = "yyyy-MM-dd HH:mm:ss"; break;
                        }
                    }
                }
               this.MetaData.PropValue = this.MetaData.PropValue.ToSafeString(true, formater);
            }

            MvcHtmlString htmlstring = this.HtmlHelper.TextBox(this.MetaData.PropName,this.MetaData.PropValue, this.MetaData.RoutValueDic);
            return htmlstring.FormItemWraperForTextBox(this.MetaData.PropName, this.MetaData.LabelName).ToHtmlString();
        }
    }
}
