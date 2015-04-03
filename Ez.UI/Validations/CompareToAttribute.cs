using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Reflection;
using System.Globalization;
using Ez.Lang;

namespace Ez.UI.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CompareToAttribute : ValidationAttribute, IClientValidatable
    {
        private string propertyName;
        public CompareToAttribute(string propertyName)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(propertyName);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult("要验证的属性不存在！");
            }
            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (!Equals(value, otherPropertyValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        public static string FormatPropertyForClientValidation(string property)
        {
            if (property == null)
            {
                throw new ArgumentException("验证参数不存在");
            }
            return "*." + property;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string ErrorMessageString = EzLanguage.ResourceManager.GetString("SYS_Val_NotEqal", System.Threading.Thread.CurrentThread.CurrentUICulture);
            yield return new ModelClientValidationEqualToRule(String.Format(CultureInfo.CurrentCulture, ErrorMessageString, metadata.DisplayName), FormatPropertyForClientValidation(propertyName));
        }

    }
}
