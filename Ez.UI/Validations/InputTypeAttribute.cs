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
    public enum InputType
    { 
        Int,
        Double,
        Number,
        DateTime
    }
    public enum DateTimeFormat
    { 
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        YYYY_MM_DD,
        /// <summary>
        /// HH-mm-ss
        /// </summary>
        HH_MM_SS,
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        YYYY_MM_DD__HH_MM_SS
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InputTypeAttribute : ValidationAttribute, IClientValidatable
    {
        private DateTimeFormat dateFormat;
        /// <summary>
        /// You can use it when the InputType's value is 'DateTime'!If not, you  will get a exception!
        /// </summary>
        public DateTimeFormat DateFormat
        {
            get {
                if (this.Dtype != InputType.DateTime)
                    throw new NotSupportedException("要求InputType类型为DateTime时可用！");
                return dateFormat; 
            }
            set {
                if (this.Dtype != InputType.DateTime)
                    throw new NotSupportedException("要求InputType类型为DateTime时可用！");
                dateFormat = value; 
            }
        }

        public InputType Dtype;

        private string formatstring {

            get {
                string regex = @"^\d{2,4}[\.|\-](0[0-9]|1[0-2])[\.|\-]([0-2][0-9]|3[0-1])$";
                switch (this.dateFormat)
                {
                    case DateTimeFormat.HH_MM_SS: regex = @"^[0-1][0-9]|2[0-4]:[0-5][0-9]:[0-5][0-9]$"; break;
                    case DateTimeFormat.YYYY_MM_DD: regex = @"^\d{2,4}[\.|\-](0[0-9]|1[0-2])[\.|\-]([0-2][0-9]|3[0-1])$"; break;
                    case DateTimeFormat.YYYY_MM_DD__HH_MM_SS: regex = @"^\d{2,4}[\.|\-](0[0-9]|1[0-2])[\.|\-]([0-2][0-9]|3[0-1])\s([0-1][0-9]|2[0-4]):[0-5][0-9]:[0-5][0-9]$"; break;
                }
                return regex;
            }
        }
        public InputTypeAttribute(InputType dtype)
        {
            this.Dtype = dtype;
        }

        /// <summary>
        /// 是否通过验证
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if(value==null) return false;
            switch (this.Dtype)
            { 
                case InputType.Int:
                  return  ValidationHelper.IsInt(value.ToString());
                case InputType.Number:
                  return ValidationHelper.IsNumber(value.ToString());
                case InputType.DateTime:
                  {
                      if(value!=null)
                      {
                          if (Type.GetTypeCode(value.GetType()) == TypeCode.DateTime)
                          {
                              return true;
                          }
                          else
                          {
                              return Regex.IsMatch((string)value, this.formatstring);
                          }
                      }
                      else 
                      return false;
                  }
                default: return false;
            }
        }
        /// <summary>
        /// 格式化错误信息
        /// </summary>
        /// <param name="name">指定的字段名</param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            string key = "";
            switch (this.Dtype)
            {
                case InputType.Int: key = "SYS_Val_Int"; break;
                case InputType.Number: key = "SYS_Val_Number"; break;
                case InputType.DateTime: key = "SYS_Val_Date"; break;
                default: key = "SYS_Val_ComMsg"; break;
            }

            string formatValue = EzLanguage.ResourceManager.GetString(key, System.Threading.Thread.CurrentThread.CurrentUICulture);
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
                ValidationType = "cktype",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["valtype"] = Dtype.ToString();//
            rule.ValidationParameters["regex"] = this.formatstring;//
            yield return rule;
        }

        #endregion
    }
}
