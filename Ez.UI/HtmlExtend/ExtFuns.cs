#define Test2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;

using Ez.Lang;
using System.Diagnostics;
using Ez.UI;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Ez.UI.HtmlExtend;
using System.Web.Routing;
using System.ComponentModel;
using Ez.UI.Entities;
namespace System.Web.Mvc
{
    internal static class TagBuilderExtensions
    {
        internal static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            Debug.Assert(tagBuilder != null);
            return MvcHtmlString.Create(tagBuilder.ToString(renderMode));
        }
        internal static void AppendTag(this TagBuilder tagBuilder, TagBuilder appendTag)
        {
            tagBuilder.AppendTag(appendTag.ToString());
        }
        internal static void AppendTag(this TagBuilder tagBuilder, string appendhtml)
        {
            tagBuilder.InnerHtml = tagBuilder.InnerHtml + appendhtml;
        }
        internal static void MergeAttributes(this TagBuilder tagBuilder, object htmlAttributes)
        {
            if (htmlAttributes != null)
            {
                RouteValueDictionary result = new RouteValueDictionary();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name.ToLower().Replace('_','-'), property.GetValue(htmlAttributes));
                }
                tagBuilder.MergeAttributes(result);
            }
            else
            {
                tagBuilder.MergeAttribute("href", "javascript:void(0)");
            }
        }
    }
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtFuns
    {
        private const string funs_id = "funs_{0}";
        /// <summary>
        /// 通过可以获取资源，只能获取框架资源，无法取得程序员定义的资源信息，不建议使用
        /// 请使用资源类名调用静态属性实现
        /// </summary>
        [Obsolete("此方法即将作废")]
        public static string Lang(this HtmlHelper html, string key)
        {
            return EzLanguage.ResourceManager.GetString(key, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// 获取用于客户端的显示文本，需要在字段上 声明 特性 CustomLabelForAttribute，不建议使用
        /// </summary>
        [Obsolete("此方法即将作废")]
        public static MvcHtmlString CustomLabelFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            MemberInfo[] members = helper.ViewData.Model.GetType().GetMembers();
            string labelvalue = "";
            foreach (MemberInfo item in members)
            {
                object[] objs = item.GetCustomAttributes(typeof(CustomLabelForAttribute), false);
                if (objs != null && objs.Length == 1 && objs[0] != null)
                {
                    CustomLabelForAttribute titleattribute = objs[0] as CustomLabelForAttribute;
                    if (titleattribute != null)
                    {
                        labelvalue = EzLanguage.ResourceManager.GetString(titleattribute.Key, System.Threading.Thread.CurrentThread.CurrentUICulture);
                        labelvalue = string.IsNullOrEmpty(labelvalue) ? titleattribute.DefaultValue : labelvalue;
                        break;
                    }
                }
            }

            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression))));
            tag.SetInnerText(labelvalue);
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        }

        /// <summary>
        /// 自定义部分视图内容
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="partViewName">视图名，请定义在~/Views/Template/目录下 文件要求有扩展名</param>
        /// <param name="model">传递给视图的数据，一般为宿主的数据模型</param>
        /// <param name="className">如果同一个模版内容有不同的样式内容那么请按标准定义后通过样式的定义名控制</param>
        public static MvcHtmlString Partial(this HtmlHelper helper, string partViewName, object model, string className)
        {
            string per = "~/Views/Template/";
            if (!partViewName.StartsWith(per))
            {
                if (partViewName.StartsWith("/")) per = per.TrimEnd('/');
                partViewName = per + partViewName;
            }
            ViewDataDictionary dic = new ViewDataDictionary();
            dic.Add("className", className);
            return Html.PartialExtensions.Partial(helper, partViewName, model, dic);
        }

        /// <summary>
        /// 生成输入框标签，在表单中框架严禁使用mvc默认的方法生成标签,因为它不具备良好的和框架配合验证的能力
        /// </summary>
        /// <typeparam name="TModel">模型</typeparam>
        /// <typeparam name="TProperty">模型的属性</typeparam>
        /// <param name="htmlHelper">扩展对象</param>
        /// <param name="expression">lamda 表达式提取属性名与值参与标签生成</param>
        /// <returns></returns>
        public static MvcHtmlString CTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes=null)
        {


            MvcHtmlString mvcgtmlstring = Html.InputExtensions.TextBoxFor(htmlHelper, expression, htmlAttributes);
            MvcHtmlString mvcgtmlstring2 = Html.ValidationExtensions.ValidationMessageFor(htmlHelper, expression);
            return MvcHtmlString.Create(mvcgtmlstring.ToHtmlString() + "<div class=\"fa val_msg\">" + mvcgtmlstring2.ToHtmlString() + "</div>");
        }

        /// <summary>
        ///  生成密码输入框标签，在表单中框架严禁使用mvc默认的方法生成标签,因为它不具备良好的和框架配合验证的能力
        /// </summary>
        /// <typeparam name="TModel">模型</typeparam>
        /// <typeparam name="TProperty">模型的属性</typeparam>
        /// <param name="htmlHelper">扩展对象</param>
        /// <param name="expression">lamda 表达式提取属性名与值参与标签生成</param>
        /// <returns></returns>
        public static MvcHtmlString CPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            MvcHtmlString mvcgtmlstring = Html.InputExtensions.PasswordFor(htmlHelper, expression, htmlAttributes);
            MvcHtmlString mvcgtmlstring2 = Html.ValidationExtensions.ValidationMessageFor(htmlHelper, expression);
            return MvcHtmlString.Create(mvcgtmlstring.ToHtmlString() + "<div class=\"fa val_msg\">" + mvcgtmlstring2.ToHtmlString() + "</div>");
        }

        /// <summary>
        /// 生成 如 下拉列表 复选框 单选框
        /// </summary>
        /// <typeparam name="TModel">模型</typeparam>
        /// <param name="htmlHelper">扩展对象</param>
        /// <param name="expression">lamda 表达式提取属性名与值参与标签生成</param>
        /// <returns></returns>
        public static MvcHtmlString SpecialControlFor<TModel>(this HtmlHelper<TModel> htmlHelper, PropertyAgent agent)
        {
            PropertyAgent ctrl = agent;
            object value = ctrl.GetAgentPropertyValue();
            int id_ = 0;
            StringBuilder html = new StringBuilder("<div class=\"ctrl_spl_box\" for=\"" + ctrl.AgentPropertyName + "\">");

            switch (ctrl.CtrlType)
            {
                case PropertyAgentType.RadioButton:
                    foreach (PropertyAgentItem item in ctrl)
                    {
                        string id = ctrl.AgentPropertyName + "_rdo" + (id_++);
                        html.Append(Html.InputExtensions.RadioButton(htmlHelper, ctrl.AgentPropertyName, item.Value, item.Selected, new { id = id }));
                        html.AppendFormat("<label for=\"{0}\">{1}</label>", id, item.Text);
                    }
                    break;
                case PropertyAgentType.CheckBox:
                    foreach (PropertyAgentItem item in ctrl)
                    {
                        string id = ctrl.AgentPropertyName + "_cbx" + (id_++);
                        html.AppendFormat("<input id=\"{0}\" name=\"{1}\" type=\"checkbox\" value=\"{2}\" {3}>", id, ctrl.AgentPropertyName, item.Value, item.Selected ? "checked" : "");//Html.InputExtensions.CheckBox(htmlHelper, ctrl.agentPropertyName, new { id = id,value=item.Value})
                        html.AppendFormat("<label for=\"{0}\">{1}</label>", id, item.Text);
                    }
                    break;
                case PropertyAgentType.DropDownList:
                    {
                        IList<SelectListItem> sel_item = new List<SelectListItem>();
                        foreach (PropertyAgentItem item in ctrl)
                        {
                            sel_item.Add(new SelectListItem()
                            {
                                Selected = item.Selected,
                                Text = item.Text,
                                Value = item.Value
                            });
                        }
                        string id = ctrl.AgentPropertyName + "_ddl" + (id_++);
                        html.Append(Html.SelectExtensions.DropDownList(htmlHelper, ctrl.AgentPropertyName, sel_item, EzLanguage.COM_N_PleaseSelect, null));
                    }
                    break;
            }
            html.Append("</div>");
            return MvcHtmlString.Create(html.ToString());
        }

        /// <summary>
        /// 生成 如 下拉列表 复选框 单选框
        /// </summary>
        /// <typeparam name="TModel">模型</typeparam>
        /// <param name="htmlHelper">扩展对象</param>
        /// <param name="expression">lamda 表达式提取属性名与值参与标签生成</param>
        /// <returns></returns>
        public static MvcHtmlString SpecialControlFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, PropertyAgent>> expression)
        {
            PropertyAgent ctrl = ModelMetadata.FromLambdaExpression<TModel, PropertyAgent>(expression, htmlHelper.ViewData).Model as PropertyAgent;
            return SpecialControlFor<TModel>(htmlHelper, ctrl);
        }

        /// <summary>
        /// 生成返回按钮
        /// </summary>
        /// <param name="htmlHelper">扩展对象</param>
        /// <returns></returns>
        public static MvcHtmlString BackBtn(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create("<input type='button' onclick='history.back(-1);' value =\"" + EzLanguage.COM_Btn_Back + "\"/>");
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString CloseBtn(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.ViewData["LayoutAction"] != null && htmlHelper.ViewContext.ViewData["LayoutAction"].ToString() == "tradition")
                return BackBtn(htmlHelper);
            else
                return MvcHtmlString.Create("<input call-close-win=\"true\" type=\"button\" value=\"" + EzLanguage.COM_Btn_Close + "\">");
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        public static MvcHtmlString SaveBtn(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create("<input  type=\"submit\" value=\"" + EzLanguage.COM_Btn_Save + "\">");
        }

        /// <summary>
        /// 产生退出按钮
        /// </summary>
        public static MvcHtmlString LoginOutLink(this HtmlHelper htmlHelper, string linkText = null, object htmlAttributes = null,bool withico =true)
        {
            RouteValueDictionary result = new RouteValueDictionary();
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    string pname = property.Name.ToLower();
                    object value = property.GetValue(htmlAttributes);
                    if (result.ContainsKey("style") && Type.GetTypeCode(value.GetType()) == TypeCode.String)
                    {
                        result["style"] = result["style"] + "" + value;
                        continue;
                    }
                    result.Add(pname, value);
                }
            }
            return A(htmlHelper, linkText, "LoginOut", "Window", withico?"fa-power-off":"", null, EzLanguage.SYS_V_LoginOut, result);
        }

        /// <summary>
        /// 创建一个连接，可为连接前设置一个图标
        /// </summary>
        public static MvcHtmlString A(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName,string beforeico, object routeValues = null, string title = null, object htmlAttributes = null)
        {
            RouteValueDictionary result = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(beforeico))
            {
                result.Add("class", "fa " + beforeico);
            }
            result.Add("style", "padding:0px 4px;");
            if (!string.IsNullOrEmpty(title))result.Add("title", title);

            
            if (htmlAttributes != null)
            {
                if (htmlAttributes is RouteValueDictionary)
                {
                    RouteValueDictionary  attrs= htmlAttributes as RouteValueDictionary;
                    if (attrs != null)
                    {
                        foreach (string key in attrs.Keys)
                        {
                            string pname = key;
                            object value = attrs[key];
                            if (result.ContainsKey("class") && Type.GetTypeCode(value.GetType()) == TypeCode.String)
                            {
                                result["class"] = value + " " + result["class"];
                                continue;
                            }
                            if (result.ContainsKey("style") && Type.GetTypeCode(value.GetType()) == TypeCode.String)
                            {
                                result["style"] = result["style"] + "" + value;
                                continue;
                            }
                            result.Add(pname, value);
                        }
                    }
                }
                else
                {
                    foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                    {
                        string pname = property.Name.ToLower();
                        object value = property.GetValue(htmlAttributes);
                        if (result.ContainsKey("class") && Type.GetTypeCode(value.GetType()) == TypeCode.String)
                        {
                            result["class"] = value + " " + result["class"];
                            continue;
                        }
                        if (result.ContainsKey("style") && Type.GetTypeCode(value.GetType()) == TypeCode.String)
                        {
                            result["style"] = result["style"] + "" + value;
                            continue;
                        }
                        result.Add(pname, value);
                    }
                }
            }
            string url = UrlHelper.GenerateUrl(null, actionName, controllerName, null, null, null, new RouteValueDictionary(routeValues), htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, false);
            TagBuilder tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!String.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : String.Empty
            };
            tagBuilder.MergeAttributes(result);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
        }
       
        /// <summary>
        /// 创建一个连接，可确定是否在标签前显示一个link图标,默认显示这个图标
        /// </summary>
        public static MvcHtmlString A(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName,bool hadBeforeLinkIco=true, object routeValues = null, string title = null, object htmlAttributes = null)
        {
            return htmlHelper.A(linkText, actionName, controllerName, hadBeforeLinkIco ? "fa-chain" : "", routeValues, title, htmlAttributes);
        }
       
        /// <summary>
        /// 创建头部菜单
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString TopBarMenu<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IList<TopMenu>>> expression, object htmlAttributes=null)
        {
            TagBuilder tag = new TagBuilder("ul");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression))));
            RouteValueDictionary result = new RouteValueDictionary();
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    string pname = property.Name.ToLower();
                    object value = property.GetValue(htmlAttributes);
                    pname = pname.Replace("_", "-");
                    result.Add(pname, value);
                }
                tag.MergeAttributes(htmlAttributes);
            }
            IList<TopMenu> list = ModelMetadata.FromLambdaExpression<TModel, IList<TopMenu>>(expression, htmlHelper.ViewData).Model as IList<TopMenu>;
            if (list == null) return null;
            StringBuilder htmlapend = new StringBuilder();
            foreach (TopMenu item in list)
            {
                TagBuilder li = new TagBuilder("li");
                li.AddCssClass("desktop-bar-menu-top");
                TagBuilder a = item.AsTagForXdialog("a");
                li.AppendTag(a);
                if (item.Children != null && item.Children.Count() > 0)
                {
                    string ul = CreateMenu(item.Children,true);
                    li.AppendTag(ul);
                }
                tag.AppendTag(li);
            }
            return tag.ToMvcHtmlString(TagRenderMode.Normal);

        }
       
        /// <summary>
        /// 创建菜单用于桌面风格布局
        /// </summary>
        /// <param name="gps"></param>
        /// <param name="isfirstchildren"></param>
        /// <returns></returns>
        static string menuid = "";
        private static string CreateMenu(IList<MenuGroup> gps,bool isfirstchildren)
        {
            if (gps == null || gps.Count == 0) return "";
            TagBuilder parent = new TagBuilder("ul");
            parent.AddCssClass(isfirstchildren ? "desktop-bar-child" : "desktop-bar-child2");
            int counter = 0;
            int menu_counter = 0;
            foreach (MenuGroup menu in gps)
            {
                foreach (Menu m in menu)
                {
                    TagBuilder _li = new TagBuilder("li");
                    _li.AddCssClass("desktop-bar-child-menu");//isfirstchildren ? "desktop-bar-child-menu" : "desktop-bar-child-menu"
                    if (isfirstchildren)
                    {
                        menuid = string.Format("_mvc_c_menu_{0}_{1}", gps.GetHashCode(), menu_counter++);
                        _li.Attributes.Add("id", menuid);
                    }
                    TagBuilder a = m.AsTagForXdialog("a", new { belong = menuid});
                    if (m.Children != null && m.Children.Count > 0)
                    {
                        a.AddCssClass("fa");
                        _li.AppendTag(CreateMenu(m.Children, false));
                    }
                    _li.AppendTag(a);
                    parent.AppendTag(_li);
                }

                if (counter < gps.Count() - 1)
                {
                    TagBuilder _split_li = new TagBuilder("li");
                    _split_li.AddCssClass("desktop-bar-menu-split");
                    parent.AppendTag(_split_li);
                }
                counter++;
            }

            return parent.ToString();
        }

        /// <summary>
        /// 获取框架要求规范的连接
        /// </summary>
        private static TagBuilder AsTagForXdialog(this Menu menu, string tagName,object attributs=null)
        {
            TagBuilder tag = new TagBuilder(tagName);
            object attributes;
            if (menu.IsWin)
            {
                attributes = new
                {
                    href = "javascript:void(0)",
                    data_src = menu.Url,
                    data_iswin = menu.IsWin,
                    data_win_size = menu.WinSize.ToString().ToLower(),
                    data_win_hasmenu = menu.UseLeftMenu,
                    data_win_hassearch = menu.UseTopSearchBar
                };
                
            }
            else
            {
                attributes = new { href = string.IsNullOrEmpty(menu.Url) ? "javascript:void(0)" : menu.Url };
            }
            tag.MergeAttributes(attributes);
            tag.MergeAttributes(menu.Attributes);
            if (!string.IsNullOrEmpty(menu.WinTitle)) tag.MergeAttribute("data-win-title", menu.WinTitle);
            if (menu.Win_Width > 0) tag.MergeAttribute("data-win-width", menu.Win_Width.ToString());
            if (menu.Win_Height > 0) tag.MergeAttribute("data-win-height", menu.Win_Height.ToString());
            if (attributs != null) tag.MergeAttributes(attributs);
            tag.SetInnerText(menu.Name);
            return tag;
        }

        /// <summary>
        /// 获取头部菜单，用于后台常规布局（上左右下结构）
        /// </summary>
        public static MvcHtmlString TopBarMenuFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IList<TopMenu>>> expression, object htmlAttributes = null)
        {

            TagBuilder tag = new TagBuilder("ul");
            tag.Attributes.Add("id", "top_bar_btns");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression))));
            RouteValueDictionary result = new RouteValueDictionary();
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    string pname = property.Name.ToLower();
                    object value = property.GetValue(htmlAttributes);
                    pname = pname.Replace("_", "-");
                    result.Add(pname, value);
                }
                tag.MergeAttributes(htmlAttributes);
            }
            IList<TopMenu> list = ModelMetadata.FromLambdaExpression<TModel, IList<TopMenu>>(expression, htmlHelper.ViewData).Model as IList<TopMenu>;
            if (list == null) return null;
            StringBuilder htmlapend = new StringBuilder();
            foreach (TopMenu item in list)
            {
                TagBuilder li = new TagBuilder("li");

                if (item.Children != null && item.Children.Count() > 0 && !string.IsNullOrEmpty(item.Id))
                {
                    li.Attributes.Add("data-children-id",string.Format(funs_id,item.Id));
                    li.Attributes.Add("data-children-count", item.Children.Count().ToString());
                }
                TagBuilder a = item.AsTagForXdialog("a");
                li.AppendTag(a);
                tag.AppendTag(li);
            }
            return tag.ToMvcHtmlString(TagRenderMode.Normal);
        
        }

        /// <summary>
        /// 获取左侧菜单，用于后台常规布局（上左右下结构）
        /// </summary>
        public static MvcHtmlString LeftMenuForTraditionLayoutFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IList<TopMenu>>> expression, object htmlAttributes = null)
        { 
             IList<TopMenu> list = ModelMetadata.FromLambdaExpression<TModel, IList<TopMenu>>(expression, htmlHelper.ViewData).Model as IList<TopMenu>;
             if (list == null) return null;
             StringBuilder htmlapend = new StringBuilder();
             int counter = 0;
             foreach (TopMenu item in list)
             {
                 if (item.Children == null) continue;
                 GetChildrenMenuBox(item.Id, item.Children, ref htmlapend, counter==0);
                 counter++;
             }
             return new MvcHtmlString(htmlapend.ToString());
        }

        /// <summary>
        /// 获取一个顶级模块对应的盒子
        /// </summary>
        private static void GetChildrenMenuBox(string id, IList<MenuGroup> gps, ref StringBuilder htmlapend, bool first)
        {
            TagBuilder tag = new TagBuilder("div");
            tag.Attributes.Add("id", string.Format(funs_id, id));
            if (!first)
            {
                tag.Attributes.Add("style", "display:none");
            }
            tag.AddCssClass("fun-list-box children-box");
            tag.InnerHtml = GetMenuBox(gps, ref htmlapend
            #if Test
                            , first
            #endif
            );
            htmlapend.Append(tag);
        }

        /// <summary>
        /// 常规化已分组的菜单集合
        /// </summary>
        private static IList<Menu> NormalMenuGroup(IList<MenuGroup> gps)
        {
            IList<Menu> menus = new List<Menu>();
            if (gps == null) return menus;
            foreach (MenuGroup item in gps)
            {
                foreach (Menu m in item)
	            {
                    menus.Add(m);
	            }
            }
            return menus;
        }

        /// <summary>
        /// 生成一个菜单模块
        /// </summary>
        private static string GetMenuBox(IList<MenuGroup> gps, ref StringBuilder htmlapend2
#if Test
            , bool first
#endif
            )
        {
            StringBuilder htmlapend = new StringBuilder();
            IList<Menu> menus = NormalMenuGroup(gps);
            foreach (Menu menu in menus)
            {
                htmlapend.Append("<div class=\"fun-wrapper\">");
#if Test
                if (first)
                {
#endif
                htmlapend.AppendFormat("<div class=\"{0}\">{1}</div>", menu.Children != null ? "am-btn am-btn-default title" : "am-btn am-btn-default title-cbtn", menu.AsTagForXdialog("a"));//---> title
                    if (menu.Children != null)
                    {
                        htmlapend.Append("<ul class=\"fun-items\">");
                        IList<Menu> menus2 = NormalMenuGroup(menu.Children);
                        int c = menus2.Count();
                        foreach (var item in menus2)
                        {
                            c--;
                            TagBuilder li = new TagBuilder("li");
                            li.AppendTag(item.AsTagForXdialog("a"));
                            if (item.Children != null)
                            {
                                li.Attributes.Add("data-children-id", string.Format(funs_id, item.Id));
                                li.Attributes.Add("data-children-count", item.Children.Count().ToString());

                                GetChildrenMenuBox(item.Id, item.Children, ref htmlapend2, false);
                            }
                            if (c > 0)
                            {
                                TagBuilder li_line = new TagBuilder("li");
                                li_line.AddCssClass("split-line");
                                htmlapend.Append(li.ToString() + li_line);
                            }
                            else
                            {
                                htmlapend.Append(li.ToString());
                            }
                        }
                        htmlapend.Append("</ul>");
                    }
#if Test
                }
                else
                {
                    TagBuilder li = new TagBuilder("li");
                    li.AddCssClass("title-cbtn");
                    li.AppendTag(menu.AsTagForXdialog("a"));
                    //htmlapend.AppendFormat("<li class=\"{0}\">{1}</li>","title-cbtn", menu.AsTagForXdialog("a"));
                    if (menu.Children != null)
                    {
                        li.Attributes.Add("data-children-id", string.Format(funs_id, menu.Id));
                        li.Attributes.Add("data-children-count", menu.Children.Count().ToString());
                        GetChildrenMenuBox(menu.Id, menu.Children, ref htmlapend2, false);
                    }
                    htmlapend.Append(li.ToString());
                }
#endif
                htmlapend.Append("</div>");
            }
            return htmlapend.ToString();
        }

        public static MvcHtmlString RenderJsonForCustomMenus(this HtmlHelper htmlHelper, CustomMenu customModel)
        {
            string br = "\r\n";
            //CustomMenu customModel = ModelMetadata.FromLambdaExpression<TModel, CustomMenu>(expression, htmlHelper.ViewData).Model as CustomMenu;
            StringBuilder jsons = new StringBuilder();
            jsons.AppendLine("[");
            IList<string> json = new List<string>();
            foreach (string title in customModel)
            {
                StringBuilder itemappend = new StringBuilder();
                itemappend.Append("{\"title\":\"" + title + "\",\"children\":[" + br);
                IList<LinkBtn> btns = customModel.GetValues(title);
                IList<string> btn = new List<string>(); 
                foreach (LinkBtn model in btns)
                {
                    btn.Add("{\"text\":\"" + model.LinkText + "\",\"src\":\"" + model.Href + "\"}");
                }
                itemappend.Append(string.Join("," + br, btn.ToArray()));
                itemappend.Append("]}" + br);
                json.Add(itemappend.ToString());
            }
            jsons.Append(string.Join("," + br, json.ToArray())+"]");
            string from = customModel.From ?? HttpContext.Current.Request.RawUrl;
            return new MvcHtmlString("setMenu(" + jsons.ToString() + ",\"" + from + "\");");
        }


        /// <summary>
        /// 生成上传标签
        /// </summary>
        /// <param name="htmlHelper">htmlHelper对象</param>
        /// <param name="uptype">上传类型枚举[image|flasg|doc|audio]</param>
        /// <param name="allownum">允许同时上传的数量</param>
        /// <param name="successfn">上传成功后回调函数</param>
        /// <param name="progressfn">进度更新函数</param>
        /// <param name="dir">指定存储目录,为空时到默认目录</param>
        /// <param name="width">按钮宽度</param>
        /// <param name="height">按钮高度</param>
        /// <param name="classStyleName">按钮使用的样式</param>
        /// <param name="auto">是否自动上传</param>
        /// <returns></returns>
        public static MvcHtmlString FileUpload(this HtmlHelper htmlHelper, FileUpLoadType uptype, int allownum, string successfn, string progressfn,string text=null, string dir="", int width = 80, int height = 20, string classStyleName = "",
            bool auto = true,object htmlAttributes = null)
        {
            TagBuilder tag = new TagBuilder("a");
            if(!string.IsNullOrEmpty(classStyleName))
            {
                tag.AddCssClass(classStyleName);
            }
            tag.MergeAttributes(new { tagway = "upload",uptype=uptype.ToString().ToLower(), data_dir = dir, data_auto = auto, data_success = successfn, data_progress = progressfn, data_allownum = allownum, width = width, height = height });
            tag.MergeAttributes(htmlAttributes);
            tag.SetInnerText(text??EzLanguage.COM_Btn_Broswer);
            return tag.ToMvcHtmlString(TagRenderMode.Normal );
        }
    }
    /// <summary>
    /// 上传文件的类型枚举
    /// </summary>
    public enum FileUpLoadType
    { 
        image,
        doc,
        excel,
        audio,
        flash
    }
}

