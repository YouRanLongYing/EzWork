using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Ez.Lang;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Web;
using System.Linq.Expressions;
using System.Web.Mvc;
namespace Ez.UI.HtmlExtend
{
    public enum PropertyAgentType
    {
        CheckBox,
        RadioButton,
        DropDownList,
        Normal
    }
    [Serializable]
    public class PropertyAgentItem : SelectListItem
    {
        public PropertyAgentItem(string text, string value, bool selected=false)
        {
            this.Text = text;
            this.Value = value;
            this.Selected = selected;
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        public new bool Selected { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public new string Value { set; get; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public new string Text { set; get; }
    }
     [Serializable]
    public class PropertyAgent
    {
        public object Parent{ set;get;}
        public bool Mutile{ set;get;}
        public PropertyAgentType CtrlType { private set; get; }
        public string AgentPropertyName { private set; get; }
        public string DisplayName { private set; get; }

        private PropertyInfo agentProperty = null;
        private TypeCode agentPropertyTypeCode;
        private IList<PropertyAgentItem> items;
        private string[] selectedval;
        /// <summary>
        /// UI控件模型，注:代理的属性类型只能为一般类型，如string、int，建议使用string避免装箱和拆箱
        /// </summary>
        /// <param name="ctrlType">展示类型</param>
        /// <param name="agentPropertyName">代理的属性名</param>
        /// <param name="mutile">是否可复选，默认不可</param>
        public PropertyAgent(PropertyAgentType ctrlType, Expression<Func<object, object>> propertyNameLambda, params string[] selectedval)
        {
           
            MemberExpression minfo = null;
            if (propertyNameLambda.Body is UnaryExpression)
            {
                minfo = (MemberExpression)((UnaryExpression)propertyNameLambda.Body).Operand;
            }
            else
            {
                minfo = (MemberExpression)propertyNameLambda.Body;
            }
            if (minfo == null)
            {
                throw new ArgumentException("Could not determine property name.", "propertyNameLambda");
            }
            this.selectedval = selectedval;
            this.AgentPropertyName = minfo.Member.Name;
            this.agentProperty = minfo.Member as PropertyInfo;
            PropertyInfo pi= (minfo).Expression.GetType().GetProperty("Value");
            if (pi!=null)
            this.Parent= pi.GetValue((minfo).Expression,null);
            this.CtrlType = ctrlType;
            if (items == null) { items = new List<PropertyAgentItem>(); }

            if (this.agentProperty == null)
            {
                string message = EzLanguage.ResourceManager.GetString("SYS_Exp_NoProperty", System.Threading.Thread.CurrentThread.CurrentUICulture);
                throw new MissingMemberException(string.Format(message,this.AgentPropertyName));
            }
            else
            {
                this.agentPropertyTypeCode = Type.GetTypeCode(this.agentProperty.PropertyType);
                if (this.agentPropertyTypeCode != TypeCode.String 
                    && this.agentPropertyTypeCode != TypeCode.Int32 
                    && !this.agentProperty.PropertyType.FullName.Equals("System.String[]")
                    && !this.agentProperty.PropertyType.Name.Equals("List`1")
                    &&!this.agentProperty.PropertyType.Name.Equals("IList`1"))
                {
                    string message = EzLanguage.ResourceManager.GetString("SYS_Exp_UnSupportType", System.Threading.Thread.CurrentThread.CurrentUICulture);
                    throw new NotSupportedException(message);
                }
                object[] objs =  this.agentProperty.GetCustomAttributes(typeof(CDisplayNameAttribute), false);
                if (objs == null || objs.Count() == 0)
                {
                    objs = this.agentProperty.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                    if (objs != null && objs.Count() == 1)
                    { 
                      DisplayNameAttribute display = objs[0] as DisplayNameAttribute;
                      this.DisplayName = display.DisplayName;
                    }
                }
                else
                {
                        CDisplayNameAttribute display = objs[0] as CDisplayNameAttribute;
                        string displayName = EzLanguage.ResourceManager.GetString(display.LangKey, System.Threading.Thread.CurrentThread.CurrentUICulture);
                        this.DisplayName = displayName ?? this.agentProperty.Name;
                }
            }
        }

        /// <summary>
        /// UI控件模型，注:代理的属性类型只能为一般类型，如string、int，建议使用string避免装箱和拆箱
        /// </summary>
        /// <param name="agentPropertyName">代理的属性名</param>
        /// <param name="mutile">是否可复选，默认不可</param>
        public PropertyAgent(Expression<Func<object, object>> propertyNameLambda, params string[] selectedval)
            : this(PropertyAgentType.Normal, propertyNameLambda, selectedval)
        { 
        
        }


        public IEnumerator GetEnumerator()
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }
        /// <summary>
        /// 代理类索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PropertyAgentItem this[int index]
        {
            get {
                return this.items[index];
            }
        }
        /// <summary>
        /// 添加一个选项
        /// </summary>
        /// <param name="item"></param>
        public void Add(PropertyAgentItem item)
        {
            if (this.selectedval != null)
            {
                item.Selected = this.selectedval.Contains(item.Value);
            }
            this.items.Add(item);
        }

        /// <summary>
        /// 选项长度
        /// </summary>
        public int Length
        {
            get
            {
                return this.items.Count();
            }
        }

        /// <summary>
        /// 按照值查找
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PropertyAgentItem FindByValue(string value)
        {
            return this.items.FirstOrDefault(p => p.Value.Equals(value));
        }

        /// <summary>
        /// 用于初始时设置相关选项的初始值，其他地方调用会修改代理选项的选中状态
        /// </summary>
        /// <param name="selectedval">为代理的属性的值</param>
        public void SetSelected(params string[] selectedval)
        {
            if (selectedval == null || selectedval[0]==string.Empty) return;
            this.selectedval = selectedval;
            foreach (PropertyAgentItem item in this)
            {
                if (this.selectedval.Contains(item.Value))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
        }

        /// <summary>
        /// 获取选中的值
        /// </summary>
        /// <returns></returns>
        public string GetSelectedValues()
        {
            if (this.CtrlType == PropertyAgentType.RadioButton)
            {
                PropertyAgentItem item = this.items.FirstOrDefault(p => p.Selected);
                return item!=null?item.Value:"";
            }
            else if (this.CtrlType == PropertyAgentType.CheckBox)
            {
                string values = HttpContext.Current.Request.Form[this.AgentPropertyName];
                if (values == null)
                {
                    return"";
                }
                IList<string> list = new List<string>();
               
                string[] vals = values.Split(',');
                foreach (string val in vals)
                {
                    if (val.ToLower() != "false")
                    {
                        this.FindByValue(val).Selected = true;
                        list.Add(val);
                    }
                }
                return string.Join(",", list.ToArray());
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 请保证泛型类型T与代理属性的类型一致
        /// </summary>
        /// <typeparam name="T">代理属性的类型</typeparam>
        /// <returns></returns>
        public T GetAgentPropertyValue<T>()
        {
            try
            {
                object value = GetAgentPropertyValue();
                T t = (T)Convert.ChangeType(value, typeof(T));
                //this.agentProperty.SetValue(this.parent, t, null);
                return t;
            }
            catch
            {
                string message = EzLanguage.ResourceManager.GetString("SYS_Exp_UnSupportType", System.Threading.Thread.CurrentThread.CurrentUICulture);
                throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// 请保证泛型类型T与代理属性的类型一致
        /// </summary>
        /// <typeparam name="T">代理属性的类型</typeparam>
        /// <returns></returns>
        public Object GetAgentPropertyValue()
        {
             return this.GetSelectedValues();
        }
    }
}
