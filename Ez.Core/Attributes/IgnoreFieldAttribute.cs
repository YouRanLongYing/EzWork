using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Core.Attributes
{
    /// <summary>
    /// 表示在映射中忽略的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreFieldAttribute:Attribute
    {
    }
}
