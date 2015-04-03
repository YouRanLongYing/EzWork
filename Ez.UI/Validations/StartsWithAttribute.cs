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
    public class StartsWithAttribute : ValidationAttribute, IClientValidatable
    {
        public string Param { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="param">指定的开头字符串</param>
        public StartsWithAttribute(string param)
        {
            Param = param;
        }


        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return ((string)value).ToLower().StartsWith(this.Param.ToLower());
        }


        /// <summary>
        /// 格式化错误信息
        /// </summary>
        /// <param name="name">指定的字段名</param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            string formatValue = EzLanguage.ResourceManager.GetString("SYS_Val_StartWith", System.Threading.Thread.CurrentThread.CurrentUICulture);
            string erroemessage = null;
            if (!string.IsNullOrEmpty(formatValue))
            {
                erroemessage = string.Format(formatValue,new object[] { name, this.Param });
            }
            return erroemessage??string.Format(ErrorMessageString, name, this.Param);
        }

        #region IClientValidatable 成员

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "startswith",//js method name
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["inputstring"] = Param;//
            yield return rule;
        }
        #endregion
    }
}
