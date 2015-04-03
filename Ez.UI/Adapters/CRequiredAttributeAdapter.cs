using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Ez.Lang;

namespace Ez.UI.Attributes
{
    public class CRequiredAttributeAdapter : RequiredAttributeAdapter
    {
        public CRequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {

        }
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var propertyName = this.Metadata.PropertyName;
            string formatstring = EzLanguage.ResourceManager.GetString("SYS_Val_Required", System.Threading.Thread.CurrentThread.CurrentUICulture);
            var erroeMessage = string.Format(formatstring, this.Metadata.DisplayName);
            if (string.IsNullOrEmpty(erroeMessage))
            {
                return base.GetClientValidationRules();
            }
            else
            {
                return new[] { new ModelClientValidationRequiredRule(erroeMessage) };
            }
        }
    }
}
