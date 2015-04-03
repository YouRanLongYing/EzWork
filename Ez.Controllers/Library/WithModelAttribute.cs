using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UBIQ.Controllers.Framework.Lib
{
    public class WithModelAttribute:Attribute
    {
        public Type ModelType { private set; get; }
        public string RequestKey { private set; get; }

        public WithModelAttribute(string RequestKey, Type modelType)
        {
            this.ModelType = modelType;
            this.RequestKey = RequestKey;
        }
    }
}
