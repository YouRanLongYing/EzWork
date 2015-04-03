using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Ez.UI.Validations;

namespace Ez.UI.Adapters
{
    public class FilterableClientDataTypeModelValidatorProvider : ClientDataTypeModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {

            var allValidators = base.GetValidators(metadata, context);
            var validators = new List<ModelValidator>();

            foreach (var v in allValidators)
            {

                if (v.GetType().Name != "NumericModelValidator")
                {
                    validators.Add(v);
                }
                else
                {
                    validators.Remove(v);
                    //NumericAttribute attribute = new NumericAttribute();
                    //DataAnnotationsModelValidator validator = new DataAnnotationsModelValidator(metadata, context, attribute);
                    //validators.Add(validator);
                }
            }

            return validators;

        }
    }
}
