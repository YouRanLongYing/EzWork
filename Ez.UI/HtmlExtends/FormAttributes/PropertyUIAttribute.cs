using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;

namespace Ez.UI.HtmlExtends.FormAttributes
{
    public abstract class PropertyUIAttribute:Attribute
    {
        public string AttributeString { set; get; }

        private bool showTitle = true;
        /// <summary>
        /// 是否在表单中显示此属性的标题
        /// </summary>
        public bool ShowTitle
        {
            get { return showTitle; }
            set { showTitle = value; }
        }

        /// <summary>
        /// UI类型
        /// </summary>
        public abstract TagType UIType { get; }

        /// <summary>
        /// 提示文本
        /// </summary>
        public string PlaceHolder { set; get; }

        /// <summary>
        /// 获取设置的特性
        /// </summary>
        /// <returns></returns>
        public virtual RouteValueDictionary GetAttributes()
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            if(!string.IsNullOrEmpty(this.PlaceHolder))
                rvd.Add("placeholder", this.PlaceHolder);
            if (!string.IsNullOrEmpty(AttributeString))
            {
                string[] keyvalue = AttributeString.Split(',');

                if (keyvalue != null && keyvalue.Length > 0)
                {
                    foreach (var kv in keyvalue)
                    {
                        string[] kvarr = kv.Split('=');
                        if (kvarr != null && kvarr.Length == 2)
                        {
                            kvarr[0] = kvarr[0].Trim();
                            kvarr[1] = kvarr[1].Trim();
                            if (rvd.ContainsKey(kvarr[0])) continue;
                            rvd.Add(kvarr[0], kvarr[1]);
                        }
                    }
                }
            }
            return rvd;
        }

    }
}
