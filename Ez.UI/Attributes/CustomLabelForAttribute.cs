using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Ez.Lang;

namespace Ez.UI
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CustomLabelForAttribute:Attribute
    {
        public string Key {private set; get; }
        public string DefaultValue { private set; get; }
        public CustomLabelForAttribute(string key, string defaultvalue = "")
        {
            this.Key = key;
            this.DefaultValue = defaultvalue;
        }

    }
}
