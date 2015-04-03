using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ez.Lang;
using Ez.Core;
namespace Ez.UI.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LessThanAttribute : ValidationAttribute, IClientValidatable
    {
        private double maxNumber;
        private bool canequal;
        /// <summary>
        /// 验证是否小于指定值
        /// </summary>
        /// <param name="maxNumber">最大值</param>
        /// <param name="canequal">是否登录最大值</param>
        public LessThanAttribute(double maxNumber, bool canequal = true)
        {
            this.maxNumber = maxNumber;
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
            double source = 0;
            if (double.TryParse(value.ToSafeString(), out source))
            {
                return this.canequal ? source <= this.maxNumber : source < this.maxNumber;
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
            string erroemessage = null;
            string formatestring = EzLanguage.ResourceManager.GetString(this.canequal ? "SYS_Val_LessThanOrEqual" : "SYS_Val_LessThan", System.Threading.Thread.CurrentThread.CurrentUICulture);
            if (!string.IsNullOrEmpty(formatestring))
            {
                erroemessage = string.Format(formatestring, this.maxNumber);
            }
            return erroemessage ?? ErrorMessageString;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "lessthan",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["max"] = this.maxNumber;//
            rule.ValidationParameters["eqmax"] = this.canequal;
            yield return rule;
        }
    }
}
