using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Reflection;

namespace UBIQ.Resource.Framework
{
    public class LanguageAssemblyProvider
    {
        public string AssemblyName { set; get; }
        public Type AssemblyType { set; get; }
    }
}
