using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Resources;
using Ez.Lang;

namespace Ez.UI.Adapters
{
    public class DisplayNameMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        private ResourceManager resourceManager = null;

        public DisplayNameMetadataProvider(ResourceManager manager)
        {
            this.resourceManager = manager;
        }
        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            string key = null;
            string defaultname = null;
            var meadata = base.GetMetadataForProperty(modelAccessor, containerType, propertyName);

            if (!string.IsNullOrEmpty(meadata.DisplayName)&&meadata.DisplayName.StartsWith("Please_fetch_from_ProResource_"))
            {
                key = meadata.DisplayName.Replace("Please_fetch_from_ProResource_", "");
                meadata.DisplayName = string.Empty;
            }
            else if (string.IsNullOrEmpty(meadata.DisplayName))
            { 
                key = string.Format("{0}_{1}", containerType.Name, propertyName);
            }
            if (!string.IsNullOrEmpty(key))
            {
                meadata.DisplayName = this.resourceManager.GetString(key);
                if (string.IsNullOrEmpty(meadata.DisplayName))
                {
                    meadata.DisplayName = string.IsNullOrEmpty(defaultname) ? meadata.PropertyName : defaultname;
                }
            }
            return meadata;
        } 
    }
}
