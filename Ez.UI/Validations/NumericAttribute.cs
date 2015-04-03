using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ez.UI.Validations
{
    /// <summary>
    /// ready http://tech.it168.com/a2012/0216/1312/000001312495.shtml
    /// </summary>
    sealed class NumericAttribute: ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return true;
        }

        IEnumerable<ModelClientValidationRule> IClientValidatable.GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule { ValidationType = "number", ErrorMessage = this.FormatErrorMessage(metadata.DisplayName) };
        }
    }

}
