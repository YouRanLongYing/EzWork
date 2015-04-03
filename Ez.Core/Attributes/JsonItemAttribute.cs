using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonItemAttribute : Attribute
    {
        private string key = "";

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        public bool PrimaryKey { set; get; }
        public JsonItemAttribute()
        { 
        }
    }
}
