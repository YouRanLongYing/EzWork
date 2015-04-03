using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using Ez.UI.HtmlExtends;
using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;
using System.Reflection;
using Ez.Core;
using Ez.UI.HtmlExtend;
using Ez.UI;
using Ez.Helper;
using Ez.UI.HtmlExtends.FormAttributes;
using Ez.UI.Validations;

namespace System.Web.Mvc
{
    /// <summary>
    /// mvchtml标签扩展类
    /// </summary>
    public static class HtmlHelperExtention
    {
        private static string GetFormTitle<TModel>(TModel model)
        {
            object[] formtag = model.GetType().GetCustomAttributes(typeof(FromTagAttribute), false);
            FromTagAttribute formtagattr = null;
            if (formtag != null && formtag.Length > 0 && (formtagattr = formtag[0] as FromTagAttribute) != null)
            {
                return formtagattr.Subject;
            }
            else
                return "";
        }
        /// <summary>
        /// 生成表单标签的主体部分
        /// </summary>
        /// <typeparam name="TModel">表单对象类型</typeparam>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="submitBtn">是否自动生成提交按钮</param>
        /// <returns></returns>
        private static string GetFormBody<TModel>(HtmlHelper<TModel> htmlHelper, bool submitBtn = true)
        {
            StringBuilder htmlappend = new StringBuilder();
            Type tModelType = htmlHelper.ViewData.Model.GetType();
            PropertyInfo[] pinfos = tModelType.GetProperties();
            //遍历类属性集合
            foreach (PropertyInfo pi in pinfos)
            {

                object[] customattr = pi.GetCustomAttributes(typeof(PropertyUIAttribute), false);//筛选UI特性
                if (customattr == null || customattr.Length <= 0) continue;
                PropertyUIAttribute attribute = customattr[0] as PropertyUIAttribute;
                string label = string.Empty;
                object[] displayattr = pi.GetCustomAttributes(typeof(CDisplayNameAttribute), false);
                CDisplayNameAttribute display = null;
                if (displayattr != null && displayattr.Length > 0 && (display = displayattr[0] as CDisplayNameAttribute) != null)
                {//设置显示文本
                    display.OnMetadataCreated(htmlHelper.ViewData.ModelMetadata);
                }
                if (attribute.ShowTitle)
                {
                    htmlHelper.ViewData.ModelMetadata.DisplayName = label = htmlHelper.ViewData.ModelMetadata.DisplayName ?? pi.Name;
                }
                //合并属性集合
                RouteValueDictionary routeDic = attribute.GetAttributes().FilterRouteValueDictionary(attribute.UIType);
                routeDic.SetValidatyAttribute(htmlHelper, pi);

                //构造用于生成标签的信息
                PropUIMetadata metedata = new PropUIMetadata
                {
                    PropInfo = pi,
                    ModelType = tModelType,
                    LabelName = label,
                    PropName = pi.Name,
                    UIType = attribute.UIType,
                    PropValue = pi.GetValue(htmlHelper.ViewData.Model, null),
                    RoutValueDic = routeDic
                };

                //累加标签的HtmlTag
                htmlappend.Append(HtmlControlFactory.Instance.CreateControlHtmlString(metedata, htmlHelper));
            }
            if (submitBtn)
            {
                htmlappend.Append("<button type=\"submit\" class=\"am-btn am-btn-primary\"><i class=\"am-icon-save\"></i>" + Ez.Lang.EzLanguage.COM_Btn_Save + "</button>");
                htmlappend.Append("<button type=\"reset\" class=\"am-btn am-btn-primary\"><i class=\"am-icon-rotate-left\"></i>" + Ez.Lang.EzLanguage.COM_Btn_Reset + "</button>");
                htmlappend.Append("<button type=\"reset\" class=\"am-btn am-btn-primary\" onclick=\"history.back(-1)\"><i class=\"am-icon-reply\"></i>" + Ez.Lang.EzLanguage.COM_Btn_Back + "</button>");
            }
            return htmlappend.ToString();
        }
        /// <summary>
        /// 生成表单标签
        /// </summary>
        /// <typeparam name="TModel">表单对象类型</typeparam>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="model">模型</param>
        /// <param name="actionName">动作</param>
        /// <param name="controllerName">控制器</param>
        /// <param name="isAsyncRequest">是否为异步提交</param>
        /// <param name="routeValues">表单传值</param>
        /// <param name="method">提交方式</param>
        /// <param name="htmlAttributes">表单标签属性</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString SmartFormFor<TModel>(this HtmlHelper<TModel> htmlHelper, TModel model, string actionName, string controllerName, bool isAsyncRequest = true, RouteValueDictionary routeValues = null, FormMethod method = FormMethod.Post, object htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("form");

            string formAction = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);
            builder.AddCssClass("am-form");
            builder.MergeAttribute("action", formAction, true);
            builder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method), true);
            if (isAsyncRequest)
            {
                builder.MergeAttribute("isajax", "true", true);
            }
            bool flag = htmlHelper.ViewContext.ClientValidationEnabled && !htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled;
            if (flag)
            {
                htmlHelper.ViewContext.FormContext.FormId = builder.Attributes["id"];
            }
            builder.MergeAttributes(htmlAttributes.ConvertToRouteValueDictionary(), true);

            StringBuilder htmlappend = new StringBuilder(builder.ToString(TagRenderMode.StartTag));

            htmlappend.Append("<fieldset>");
            htmlappend.AppendFormat("<legend>{0}</legend>", GetFormTitle(model));
            htmlappend.Append(GetFormBody(htmlHelper));
            htmlappend.Append("</fieldset>");
            htmlappend.Append("</form>");
            return MvcHtmlString.Create(htmlappend.ToString());
        }
        /// <summary>
        /// 生成表单标签
        /// </summary>
        /// <typeparam name="TModel">表单对象类型</typeparam>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="model">模型</param>
        /// <param name="htmlAttributes">表单标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString SmartFormFor<TModel>(this HtmlHelper<TModel> htmlHelper, TModel model, object htmlAttributes = null)
        {
            object[] formAttr = htmlHelper.ViewData.Model.GetType().GetCustomAttributes(typeof(FromTagAttribute), false);
            if (formAttr != null && formAttr.Length == 1)
            {
                FromTagAttribute formattr = formAttr[0] as FromTagAttribute;
                return SmartFormFor(htmlHelper, model, formattr.ActionName, formattr.ControllerName, formattr.IsAsyncSubmit, formattr.RouteValues, formattr.Method, htmlAttributes);
            }
            else
            {
                return new MvcHtmlString("");
            }
        }

        /// <summary>
        /// 生成表单标签
        /// </summary>
        /// <typeparam name="TModel">表单对象类型</typeparam>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="model">模型</param>
        /// <param name="actionName">动作</param>
        /// <param name="controllerName">控制器</param>
        /// <param name="isAsyncSubmit">是否为异步提交</param>
        /// <param name="routeValues">表单传值</param>
        /// <param name="method">提交方式</param>
        /// <param name="htmlAttributes">表单标签属性</param>
        /// <returns>MvcForm</returns>
        public static MvcForm SmartFormBeginFor<TModel>(this HtmlHelper<TModel> htmlHelper, TModel model, string actionName, string controllerName, bool isAsyncSubmit = true, RouteValueDictionary routeValues = null, FormMethod method = FormMethod.Post, object htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("form");
            builder.AddCssClass("am-form");
            builder.MergeAttributes(htmlAttributes);
            string formAction = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);
            builder.MergeAttribute("action", formAction, true);
            builder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method), true);
            if (isAsyncSubmit)
            {
                builder.MergeAttribute("isajax", "true", true);
            }
            bool flag = htmlHelper.ViewContext.ClientValidationEnabled && !htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled;
            if (flag)
            {
                htmlHelper.ViewContext.FormContext.FormId = builder.Attributes["id"];
            }
            htmlHelper.ViewContext.Writer.Write(builder.ToString(TagRenderMode.StartTag));
            htmlHelper.ViewContext.Writer.Write("<fieldset>");
            htmlHelper.ViewContext.Writer.Write("<legend>" + GetFormTitle(model) + "</legend>");
            htmlHelper.ViewContext.Writer.Write(GetFormBody(htmlHelper, false));
            //htmlHelper.ViewContext.Writer.Write("</fieldset>");转移到 Dispose 中输出
            return new HtmlMvcForm(htmlHelper.ViewContext);
        }
        /// <summary>
        /// 生成表单标签
        /// </summary>
        /// <typeparam name="TModel">表单对象类型</typeparam>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="model">模型</param>
        /// <param name="htmlAttributes">表单标签属性</param>
        /// <returns>MvcForm</returns>
        public static MvcForm SmartFormBeginFor<TModel>(this HtmlHelper<TModel> htmlHelper, TModel model, object htmlAttributes = null)
        {
            object[] formAttr = htmlHelper.ViewData.Model.GetType().GetCustomAttributes(typeof(FromTagAttribute), false);
            if (formAttr != null && formAttr.Length == 1)
            {
                FromTagAttribute formattr = formAttr[0] as FromTagAttribute;
                return SmartFormBeginFor(htmlHelper, model, formattr.ActionName, formattr.ControllerName, formattr.IsAsyncSubmit, formattr.RouteValues, formattr.Method, htmlAttributes);
            }
            else
            {
                return new MvcForm(htmlHelper.ViewContext);
            }
        }
    }

    /// <summary>
    /// mvchtml标签扩展类
    /// </summary>
    public static class HtmlControlExtention
    {
        private static MvcHtmlString EzSmartControl<TModel, TProperty>(TagType uiType, HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            PropUIMetadata metaData = htmlHelper.GetPropertyMetadata(expression, uiType);
            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }

        /// <summary>
        /// 根据模型属性标有的特性（*UI）生成对应标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzSmartControl<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            PropUIMetadata metaData = htmlHelper.GetPropertyMetadata(expression);
            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }

        /// <summary>
        /// 生成input标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">input的name</param>
        /// <param name="value">input的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzTextBox(this HtmlHelper htmlHelper, string name, string value,string label="", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.TextBox,
                RoutValueDic = htmlAttributes.ConvertToRouteValueDictionary(),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };
            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成input标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.TextBox, htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 生成textarea标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">textarea的name</param>
        /// <param name="value">textarea的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzTextArea(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.TextArea,
                RoutValueDic = htmlAttributes.ConvertToRouteValueDictionary(),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };
            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成textarea标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.TextArea, htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 生成富文本编辑器标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">Editor的name</param>
        /// <param name="value">Editor的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzHtmlEditor(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.Editor,
                RoutValueDic = (new EditorUIAttribute()).GetAttributes().MergeWithRouteValueDictionary(htmlAttributes.ConvertToRouteValueDictionary(),true),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };
            
            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成富文本编辑器标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzHtmlEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.Editor, htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 生成DatePicker标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">DatePicker的name</param>
        /// <param name="value">DatePicker的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzDatePicker(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.DatePicker,
                RoutValueDic = (new DatePickerUIAttribute()).GetAttributes().MergeWithRouteValueDictionary(htmlAttributes.ConvertToRouteValueDictionary(), true),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };

            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成DatePicker标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzDatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.DatePicker, htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 生成Password标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">Password的name</param>
        /// <param name="value">Password的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzPassword(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.Password,
                RoutValueDic = (new PasswordUIAttribute()).GetAttributes().MergeWithRouteValueDictionary(htmlAttributes.ConvertToRouteValueDictionary(), true),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };

            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成Password标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.Password, htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 生成Hidden标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">Hidden的name</param>
        /// <param name="value">Hidden的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzHidden(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.Hidden,
                RoutValueDic = (new HiddenUIAttribute()).GetAttributes().MergeWithRouteValueDictionary(htmlAttributes.ConvertToRouteValueDictionary(), true),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };

            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成Hidden标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.Hidden, htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 生成UploadFile标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">UploadFile的name</param>
        /// <param name="value">UploadFile的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzUploadFile(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.UploadFile,
                RoutValueDic = (new UploadFileUIAttribute()).GetAttributes().MergeWithRouteValueDictionary(htmlAttributes.ConvertToRouteValueDictionary(), true),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };

            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成UploadFile标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzUploadFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.UploadFile, htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 生成Radio标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">Radio的name</param>
        /// <param name="value">Radio的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzRadio(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.Radio,
                RoutValueDic = (new RadioUIAttribute()).GetAttributes().MergeWithRouteValueDictionary(htmlAttributes.ConvertToRouteValueDictionary(), true),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };

            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成Radio标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzRadioFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.Radio, htmlHelper, expression, htmlAttributes);
        }

        /// <summary>
        /// 生成CheckBox标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">CheckBox的name</param>
        /// <param name="value">CheckBox的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzCheckBox(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.CheckBox,
                RoutValueDic = (new CheckBoxUIAttribute()).GetAttributes().MergeWithRouteValueDictionary(htmlAttributes.ConvertToRouteValueDictionary(), true),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };

            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成CheckBox标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.CheckBox, htmlHelper, expression, htmlAttributes);
        }
        /// <summary>
        /// 生成Select标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="name">Select的name</param>
        /// <param name="value">Select的value</param>
        /// <param name="label">标题文本</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzSelect(this HtmlHelper htmlHelper, string name, string value, string label = "", object htmlAttributes = null)
        {
            PropUIMetadata metaData = new PropUIMetadata
            {
                LabelName = label,
                UIType = TagType.Select,
                RoutValueDic = (new SelectUIAttribute()).GetAttributes().MergeWithRouteValueDictionary(htmlAttributes.ConvertToRouteValueDictionary(), true),
                ModelType = null,
                PropInfo = null,
                PropName = name,
                PropValue = value
            };

            string html = HtmlControlFactory.Instance.CreateControlHtmlString(metaData, htmlHelper);
            return MvcHtmlString.Create(html);
        }
        /// <summary>
        /// 使用视图模型属性生成Select标签
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="expression">属性表达式</param>
        /// <param name="htmlAttributes">标签属性</param>
        /// <returns></returns>
        public static MvcHtmlString EzSelectFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return EzSmartControl(TagType.Select, htmlHelper, expression, htmlAttributes);
        }
    }
}
