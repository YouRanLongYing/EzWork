using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ez.Lang;
using Ez.Helper;

namespace Ez.UI.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RangeLenAttribute : ValidationAttribute, IClientValidatable
    {
         private int minLen;
         private int maxLen;
        private bool canEqualMin;
        private bool canEqualMax;
        public RangeLenAttribute(int minLen, int maxLen, bool canEqualMin = true, bool canEqualMax = true)
        {
            this.minLen = minLen;
            this.maxLen = maxLen;
            this.canEqualMin = canEqualMin;
            this.canEqualMax = canEqualMax;
        }
        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return ValidationHelper.IsLengthStr((string)value, this.minLen, this.maxLen,this.canEqualMin,this.canEqualMax);
        }
        /// <summary>
        /// 格式化错误信息
        /// </summary>
        /// <param name="name">指定的字段名</param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            string erroemessage=null;
            string key = "SYS_Val_ComMsg";
            if (this.canEqualMax && this.canEqualMin)
            {
                key = "SYS_Val_RangeLenOrEqual";
            }
            else if (!this.canEqualMax && this.canEqualMin)
            {
                key = "SYS_Val_RangeLenOrEqMin";
            }
            else if (this.canEqualMax && !this.canEqualMin)
            {
                key = "SYS_Val_RangeLenOrEqMax";
            }
            else
            {
                key = "SYS_Val_RangeLen";
            }
            string formatestring = EzLanguage.ResourceManager.GetString(key, System.Threading.Thread.CurrentThread.CurrentUICulture);
            if (!string.IsNullOrEmpty(formatestring))
            {
                erroemessage = string.Format(formatestring, this.minLen, this.maxLen);
            }
            return erroemessage ?? ErrorMessageString;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "rangelenth",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["min"] = this.minLen;//
            rule.ValidationParameters["max"] = this.maxLen;//
            rule.ValidationParameters["eqmin"] = this.canEqualMin;//
            rule.ValidationParameters["eqmax"] = this.canEqualMin;//
            yield return rule;
        }
    }
}
