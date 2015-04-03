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
    public class TextAreaUIAttribute : PropertyUIAttribute
    {
        public override TagType UIType { get { return TagType.TextArea; } }
        /// <summary>
        /// 允许行数
        /// </summary>
        public int Rows { set; get; }
        /// <summary>
        /// 允许列数
        /// </summary>
        public int Columns { set; get; }
        /// <summary>
        /// 获取设置的特性
        /// </summary>
        /// <returns></returns>
        public override RouteValueDictionary GetAttributes()
        {
            RouteValueDictionary routeValDic = base.GetAttributes();
            if (this.Rows > 0) routeValDic.Add("row", this.Rows);
            if (this.Columns > 0) routeValDic.Add("columns", this.Columns);
            return routeValDic;
        }
    }
}
