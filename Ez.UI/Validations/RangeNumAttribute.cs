using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ez.Helper;
using Ez.Lang;

namespace Ez.UI.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RangeNumAttribute : ValidationAttribute, IClientValidatable
    {
        private double minNumber;
        private double maxNumber;
        private bool canEqualMin;
        private bool canEqualMax;
        public RangeNumAttribute(double minNumber, double maxNumber,bool canEqualMin=true,bool canEqualMax =true)
        {
            this.minNumber = minNumber;
            this.maxNumber = maxNumber;
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
            if (value == null) return false;
            double source=0;
            if (double.TryParse((string)value, out source))
            {
                return source >= this.minNumber && source <= this.maxNumber;
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
            string key = "SYS_Val_ComMsg";
            if (this.canEqualMax && this.canEqualMin)
            {
                key = "SYS_Val_RangeOrEqual";
            }
            else if (!this.canEqualMax && this.canEqualMin)
            {
                key = "SYS_Val_RangeOrEqMin";
            }
            else if (this.canEqualMax && !this.canEqualMin)
            {
                key = "SYS_Val_RangeOrEqMax";
            }
            else
            {
                key = "SYS_Val_Range";
            }
            string erroemessage=null;
            string formatestring = EzLanguage.ResourceManager.GetString(key, System.Threading.Thread.CurrentThread.CurrentUICulture);
            if (!string.IsNullOrEmpty(formatestring))
            {
                erroemessage = string.Format(formatestring, this.minNumber, this.maxNumber);
            }
            return erroemessage ?? ErrorMessageString;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "ranges",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["min"] = this.minNumber;//
            rule.ValidationParameters["max"] = this.maxNumber;//
            rule.ValidationParameters["eqmin"] = this.canEqualMin;//
            rule.ValidationParameters["eqmax"] = this.canEqualMin;//
            yield return rule;
        }
    
    }
}
