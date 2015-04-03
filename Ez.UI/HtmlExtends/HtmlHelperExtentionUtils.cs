using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Ez.UI.Validations;
using System.Web.Mvc.Html;
using System.ComponentModel.DataAnnotations;
using Ez.UI.HtmlExtends;
using System.Collections;
using Ez.UI.Attributes;
using Ez.UI;
using Ez.UI.HtmlExtends.FormAttributes;
using Ez.Core.Interceptor;
namespace System.Web.Mvc
{
    public static class HtmlHelperExtentionUtils
    {
        internal static MvcHtmlString FormItemWraperForHidden(this MvcHtmlString mvcHtmlString)
        {
            return new MvcHtmlString(string.Format("<div style=\"display:none;\">{0}</div>", mvcHtmlString.ToHtmlString()));
        }
        /// <summary>
        /// 生成表单输入项包裹
        /// </summary>
        internal static MvcHtmlString FormItemWraperForTextBox(this MvcHtmlString mvcHtmlString, string id, string wraperName)
        {
            return new MvcHtmlString(string.Format("<div class=\"am-form-group\">\r<label for=\"{2}\">{0}</label>\r{1}</div>", wraperName, mvcHtmlString.ToHtmlString(), id));
        }
        /// <summary>
        /// 生成表单输入项包裹
        /// </summary>
        internal static MvcHtmlString FormItemWraperForRadioOrCheckbox(this MvcHtmlString mvcHtmlString, string wraperName)
        {
            if (string.IsNullOrEmpty(wraperName))
            {
                return new MvcHtmlString(string.Format("<div class=\"am-form-group\"><div>\r{0}\r</div>\r</div>", mvcHtmlString.ToHtmlString()));
            }
            else
            {
                return new MvcHtmlString(string.Format("<div class=\"am-form-group\">\r<label>{0}</label>\r<div>\r{1}\r</div>\r</div>", wraperName, mvcHtmlString.ToHtmlString()));
            }
        }
        private static string GetInputType(TagType InputTagType)
        {
            string typestr = "";
            switch (InputTagType)
            {
                case TagType.TextBox: typestr = "text"; break;
                case TagType.Password: typestr = "password"; break;
                case TagType.Radio: typestr = "radio"; break;
                case TagType.CheckBox: typestr = "checkbox"; break;
                case TagType.UploadFile: typestr = "file"; break;
            }
            return typestr;
        }

        /// <summary>
        /// 过滤标签属性
        /// </summary>
        public static RouteValueDictionary FilterRouteValueDictionary(this RouteValueDictionary htmlAttributes, TagType InputTagType)
        {
            htmlAttributes = htmlAttributes ?? new RouteValueDictionary();
            string typestr = GetInputType(InputTagType);
            if ((htmlAttributes == null || !htmlAttributes.ContainsKey("type")) && !string.IsNullOrEmpty(typestr))
            {
                htmlAttributes.Add("type", typestr);
            }
            return htmlAttributes;
        }
        /// <summary>
        /// 获取标签属性
        /// </summary>
        public static RouteValueDictionary ConvertToRouteValueDictionary(this object htmlAttributes)
        {
            if (htmlAttributes is RouteValueDictionary)
            {
                return htmlAttributes as RouteValueDictionary;
            }
            else
            {
                RouteValueDictionary result = new RouteValueDictionary();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name, property.GetValue(htmlAttributes));
                }
                return result;
            }
        }
        /// <summary>
        /// 合并两个字典集合
        /// </summary>
        /// <param name="RouteDicTarget">要合并到的集合</param>
        /// <param name="RouteDicSource">被合并的集合</param>
        /// <param name="replaceExists">是否替换掉已存在的键对应的值</param>
        /// <returns>合并好的集合</returns>
        public static RouteValueDictionary MergeWithRouteValueDictionary(this RouteValueDictionary routeDicTarget, IDictionary<string, object> source, bool replaceExists)
        {

            routeDicTarget = routeDicTarget ?? new RouteValueDictionary();

            if (routeDicTarget != null)
            {
                foreach (string key in source.Keys)
                {
                    if (routeDicTarget.ContainsKey(key) && replaceExists)
                    {
                        routeDicTarget[key] = source[key];
                    }
                    else if (!routeDicTarget.ContainsKey(key))
                    {
                        routeDicTarget.Add(key, source[key]);
                    }
                }
            }
            return routeDicTarget;
        }
        /// <summary>
        /// 合并两个字典集合
        /// </summary>
        /// <param name="RouteDicTarget">要合并到的集合</param>
        /// <param name="RouteDicSource">被合并的集合</param>
        /// <param name="replaceExists">是否替换掉已存在的键对应的值</param>
        /// <returns>合并好的集合</returns>
        public static RouteValueDictionary MergeWithRouteValueDictionary(this RouteValueDictionary routeDicTarget, RouteValueDictionary source, bool replaceExists)
        {
            routeDicTarget = routeDicTarget ?? new RouteValueDictionary();
            if (routeDicTarget != null)
            {
                foreach (string key in source.Keys)
                {
                    if (routeDicTarget.ContainsKey(key) && replaceExists)
                    {
                        routeDicTarget[key] = source[key];
                    }
                    else if (!routeDicTarget.ContainsKey(key))
                    {
                        routeDicTarget.Add(key, source[key]);
                    }
                }
            }
            return routeDicTarget;
        }


        private static PropertyUIAttribute GetPropertyUIAttribute(TagType uitype)
        {
            PropertyUIAttribute propuiattr = null;
            string typename = "Ez.UI.HtmlExtends.FormAttributes." + uitype + "UIAttribute";
            try
            {
                Object instace = Assembly.Load("EzUI").CreateInstance(typename);
                propuiattr = instace as PropertyUIAttribute;
            }
            catch (Exception exp)
            {
                Log4NetManager.Output(new ExecuteInfo
                {
                    Args = new object[] { "UI", typename },
                    Exception = exp,
                    LogLevel = LogLevel.Error,
                    IsEndPoint = false,
                    MethodName = " Assembly.Load or GetPropertyUIAttribute"
                });
            }
            return propuiattr;
        }


        public static PropUIMetadata GetPropertyMetadata<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, TagType uitype =  TagType.Normal)
        {
            PropertyInfo pi = htmlHelper.ViewContext.ViewData.Model.GetType().GetProperty(ExpressionHelper.GetExpressionText(expression));
            object[] displayattr = pi.GetCustomAttributes(typeof(CDisplayNameAttribute), false);
            CDisplayNameAttribute display = null;
            if (displayattr != null && displayattr.Length > 0 && (display = displayattr[0] as CDisplayNameAttribute) != null)
            {//设置显示文本
                display.OnMetadataCreated(htmlHelper.ViewData.ModelMetadata);
            }
            //构造用于生成标签的信息
            PropUIMetadata metedata = new PropUIMetadata
            {
                PropInfo = pi,
                ModelType = htmlHelper.ViewContext.ViewData.Model.GetType(),
                PropName = pi.Name,
                PropValue = pi.GetValue(htmlHelper.ViewData.Model, null),
            };
            PropertyUIAttribute uiAttribute=null;

            if (uitype != TagType.Normal)
            {
                uiAttribute = GetPropertyUIAttribute(uitype);
            }

            if (uiAttribute == null)
            {
                object[] customattr = pi.GetCustomAttributes(typeof(PropertyUIAttribute), false);//筛选UI特性
                if (customattr != null && customattr.Length > 0)
                {
                    uiAttribute = customattr[0] as PropertyUIAttribute;
                }
            }

            if (uiAttribute.ShowTitle)
            {
                htmlHelper.ViewData.ModelMetadata.DisplayName = metedata.LabelName = htmlHelper.ViewData.ModelMetadata.DisplayName ?? pi.Name;
            }
            metedata.UIType = uiAttribute.UIType;
            //合并属性集合
            RouteValueDictionary routeDic = uiAttribute.GetAttributes().FilterRouteValueDictionary(uiAttribute.UIType);
            routeDic.SetValidatyAttribute(htmlHelper, pi);
            metedata.RoutValueDic = routeDic;

            return metedata;
        }


        public static RouteValueDictionary SetValidatyAttribute<TModel>(this RouteValueDictionary routeValueDic, HtmlHelper<TModel> htmlHelper, PropertyInfo propInfo)
        {
            routeValueDic = routeValueDic ?? new RouteValueDictionary();

            object[] requireval = propInfo.GetCustomAttributes(typeof(RequiredAttribute), false);
            if (requireval != null && requireval.Length > 0 && requireval[0] is RequiredAttribute)
            {
                RequiredAttribute valdaty = requireval[0] as RequiredAttribute;
                CRequiredAttributeAdapter adapter = new CRequiredAttributeAdapter(htmlHelper.ViewData.ModelMetadata, htmlHelper.ViewContext.Controller.ControllerContext, valdaty);
                IEnumerable<ModelClientValidationRule> rules = adapter.GetClientValidationRules();
                SetValidatyAttribute(rules, routeValueDic);
            }

            object[] clientval = propInfo.GetCustomAttributes(typeof(IClientValidatable), false);
            if (clientval != null && clientval.Length > 0 && clientval[0] is IClientValidatable)
            {
                IClientValidatable valdaty = clientval[0] as IClientValidatable;
                IEnumerable<ModelClientValidationRule> rules = valdaty.GetClientValidationRules(htmlHelper.ViewData.ModelMetadata, htmlHelper.ViewContext.Controller.ControllerContext);
                SetValidatyAttribute(rules, routeValueDic);
            }
            return routeValueDic;
        }

        private static RouteValueDictionary SetValidatyAttribute(IEnumerable<ModelClientValidationRule> rules, RouteValueDictionary routeValueDic)
        {
            if (!routeValueDic.ContainsKey("data-val")) routeValueDic.Add("data-val", "true");
            foreach (ModelClientValidationRule rule in rules)
            {
                string valtype = string.Format("data-val-{0}", rule.ValidationType);
                routeValueDic.Add(valtype, rule.ErrorMessage);
                foreach (string key in rule.ValidationParameters.Keys)
                {
                    routeValueDic.Add(string.Format("{0}-{1}", valtype, key), rule.ValidationParameters[key]);
                }
            }
            return routeValueDic;
        }
    }
}
