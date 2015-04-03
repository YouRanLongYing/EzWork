using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Reflection;
using System.Globalization;
using System.Resources;


namespace Ez.Lang
{
    public class CResourceManager : System.Resources.ResourceManager
    {
        public CResourceManager(string baseName, Assembly assembly)
            : base(baseName, assembly)
        {

        }

        public ResourceManager CustomResourceManager { set; get; }
        public override string GetString(string name,System.Globalization.CultureInfo culture)
        {
            string value = base.GetString(name, culture);
            if (string.IsNullOrEmpty(value) && this.CustomResourceManager != null)
            {
                value = this.CustomResourceManager.GetString(name, culture);
            }
            return value;
        }

    }
}
