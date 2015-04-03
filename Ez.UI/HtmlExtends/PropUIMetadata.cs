using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Reflection;
using System.Linq.Expressions;

namespace Ez.UI.HtmlExtends
{

    public class PropUIMetadata
    {
        /// <summary>
        /// 控件类型
        /// </summary>
        public TagType UIType { set; get; }

        /// <summary>
        /// 模型类型
        /// </summary>
        public Type ModelType { set; get; }

        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo PropInfo { set; get; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropName { set; get; }
        /// <summary>
        /// 属性值
        /// </summary>
        public object PropValue { set; get; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string LabelName { set; get; }

        /// <summary>
        /// 标签的属性如 style="..."
        /// </summary>
        public RouteValueDictionary RoutValueDic { set; get; }
    }
}
