using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ez.Lang;

namespace Ez.UI.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        private double minNumber;
        private bool canequal;
        /// <summary>
        /// 验证是否小于指定值
        /// </summary>
        /// <param name="maxNumber">最大值</param>
        /// <param name="canequal">是否登录最大值</param>
        public GreaterThanAttribute(double minNumber, bool canequal = true)
        {
            this.minNumber = minNumber;
            this.canequal = canequal;
        }
        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            double source=0;
            if (double.TryParse((string)value, out source))
            {
                return this.canequal ? source >= this.minNumber : source > this.minNumber;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 格式化错误信息
        /// </summary>
        /// <param name="name">指定的字段名</param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            string erroemessage=null;
            string formatestring = EzLanguage.ResourceManager.GetString(this.canequal ? "SYS_Val_GreaterThanOrEqual" : "SYS_Val_GreaterThan", System.Threading.Thread.CurrentThread.CurrentUICulture);
            if (!string.IsNullOrEmpty(formatestring))
            {
                erroemessage = string.Format(formatestring,this.minNumber);
            }
            return erroemessage ?? ErrorMessageString;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "greaterthan",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["min"] = this.minNumber;//
            rule.ValidationParameters["eqmin"] = this.canequal;
            yield return rule;
        }
    }
}
