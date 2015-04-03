using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Ez.Helper;
using Ez.Lang;

namespace Ez.UI.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PhoneNumberAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return ValidationHelper.IsPhoneNumber((string)value);
        }
        /// <summary>
        /// 格式化错误信息
        /// </summary>
        /// <param name="name">指定的字段名</param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            string erroemessage = EzLanguage.ResourceManager.GetString("SYS_Val_PhoneNumber", System.Threading.Thread.CurrentThread.CurrentUICulture);

            return erroemessage ?? ErrorMessageString;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "phone",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            yield return rule;
        }
    
    }
}
