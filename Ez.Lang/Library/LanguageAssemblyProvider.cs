using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Reflection;

namespace Ez.Lang
{
    public class LanguageAssemblyProvider
    {
        public string AssemblyName { set; get; }
        public Type AssemblyType { set; get; }
    }
}
