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
    public class DatePickerUIAttribute : PropertyUIAttribute
    {
        /// <summary>
        /// 标签附加属性
        /// </summary>
        public string DateFormater { set; get; }

        public override TagType UIType { get { return TagType.DatePicker; } }

        private bool readOnly = true;
        /// <summary>
        /// 是否只读,默认只读
        /// </summary>
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }



        public override RouteValueDictionary GetAttributes()
        {
            RouteValueDictionary rvd = base.GetAttributes();
            rvd.Add("data-am-datepicker",this.DateFormater);
            if (this.ReadOnly)
            {
                rvd.Add("readonly", "readonly");
            }
            return rvd;
        }
    }
}
