using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Ez.UI.Validations;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ez.UI.HtmlExtends.FormAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EditorUIAttribute : PropertyUIAttribute
    {
        public override TagType UIType { get { return TagType.Editor; } }
        /// <summary>
        /// 高度，不设置则使用默认方式
        /// </summary>
        public int Height { set; get; }
        /// <summary>
        /// 宽度，不设置则使用默认方式
        /// </summary>
        public int Width { set; get; }
        /// <summary>
        /// 获取设置的特性
        /// </summary>
        /// <returns></returns>
        public override RouteValueDictionary GetAttributes()
        {
            RouteValueDictionary rvd = base.GetAttributes();
            rvd.Add("editor", "yes");
            if (this.Height > 0) rvd.Add("row", this.Height);
            if (this.Width > 0) rvd.Add("columns", this.Width);
            return rvd;
        }
    }
}
