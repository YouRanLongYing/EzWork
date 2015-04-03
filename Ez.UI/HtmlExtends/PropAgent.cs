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
namespace Ez.UI.HtmlExtends
{
    [Serializable]
    public class PropAgentItem : SelectListItem
    {
        public PropAgentItem() { }
        public PropAgentItem(string text, string value, bool selected = false)
        {
            this.Text = text;
            this.Value = value;
            this.Selected = selected;
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool selected;

        public new bool Selected
        {
            get { return selected; }
            set { 
                selected = value;
                base.Selected = value;
            }
        }

       
        private string value;
        /// <summary>
        /// 值
        /// </summary>
        public new string Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                base.Value = value; }
        }

        private string text;
        /// <summary>
        /// 显示文本
        /// </summary>
        public new string Text
        {
            get { return text; }
            set { 
                text = value;
                base.Text = value;
            }
        }

    }
    [Serializable]
    public class PropAgent : IEnumerable<SelectListItem>
    {
        public PropAgent() { if (items == null) { items = new List<PropAgentItem>(); } }
        public string PropName { private set; get; }
        private IList<PropAgentItem> items;
        /// <summary>
        /// UI控件模型，注:代理的属性类型只能为一般类型，如string、int，建议使用string避免装箱和拆箱
        /// </summary>
        /// <param name="ctrlType">展示类型</param>
        /// <param name="agentPropertyName">代理的属性名</param>
        public PropAgent(Expression<Func<object, object>> propNameLambda)
        {
           
            MemberExpression minfo = null;
            if (propNameLambda.Body is UnaryExpression)
            {
                minfo = (MemberExpression)((UnaryExpression)propNameLambda.Body).Operand;
            }
            else
            {
                minfo = (MemberExpression)propNameLambda.Body;
            }
            if (minfo == null)
            {
                throw new ArgumentException("Could not determine property name.", "propertyNameLambda");
            }
            this.PropName = minfo.Member.Name;

            if (items == null) { items = new List<PropAgentItem>(); }
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
        public PropAgentItem this[int index]
        {
            get {
                return this.items[index];
            }
        }
        /// <summary>
        /// 添加一个选项
        /// </summary>
        /// <param name="item"></param>
        public void Add(PropAgentItem item)
        {
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
        public PropAgentItem FindByValue(string value)
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
            foreach (PropAgentItem item in this)
            {
                if (selectedval.Contains(item.Value))
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
            string values = HttpContext.Current.Request.Form[this.PropName];
            if (values == null)
            {
                return "";
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

        IEnumerator<SelectListItem> IEnumerable<SelectListItem>.GetEnumerator()
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }
    }
}
