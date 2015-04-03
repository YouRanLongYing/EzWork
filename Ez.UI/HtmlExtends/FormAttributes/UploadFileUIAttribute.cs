using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Linq.Expressions;

namespace Ez.UI.HtmlExtends.FormAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UploadFileUIAttribute : PropertyUIAttribute
    {
        /// <summary>
        /// UI类型
        /// </summary>
        public override TagType UIType
        {
            get
            {
                return TagType.UploadFile;
            }
        }
        /// <summary>
        /// 允许上传的数量
        /// </summary>
        public int Allownum { set; get; }

        /// <summary>
        /// 是否自动上传
        /// </summary>
        public bool Auto { set; get; }

        /// <summary>
        /// 获取属性结合
        /// </summary>
        /// <returns></returns>
        public override RouteValueDictionary GetAttributes()
        {
            RouteValueDictionary rvd = base.GetAttributes();
            rvd.Add("tagway", "upload");
            rvd.Add("data_auto", this.Auto);
            rvd.Add("data_allownum", this.Allownum);
            rvd.Add("placeholder", this.PlaceHolder);
            return rvd;
        }
    }
}
