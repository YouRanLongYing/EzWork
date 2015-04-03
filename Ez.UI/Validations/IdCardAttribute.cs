using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ez.Helper;
using Ez.Lang;
using System.Text.RegularExpressions;

namespace Ez.UI.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IdCardAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return ValidationHelper.IsIdCard((string)value);
        }
        /// <summary>
        /// 格式化错误信息
        /// </summary>
        /// <param name="name">指定的字段名</param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            string formatValue = EzLanguage.ResourceManager.GetString("SYS_Val_IdCard", System.Threading.Thread.CurrentThread.CurrentUICulture);
            string erroemessage = null;
            if (!string.IsNullOrEmpty(formatValue))
            {
                erroemessage = string.Format(formatValue, name);
            }
            return erroemessage ?? string.Format(ErrorMessageString, name);
        }

        #region IClientValidatable 成员

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "idcard",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            yield return rule;
        }

        #endregion
    }
}
