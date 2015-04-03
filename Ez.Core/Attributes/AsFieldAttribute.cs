using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Core.Attributes
{
    /// <summary>
    /// ORM 映射属性为数据库表的字段，未指定则使用属性名作为字段名使用（请确保属性名与字段名一致否则会报错）
    /// 注意：如果想忽略某个属性请在属性添加特性IgnoreField，否则即使没有标明为映射字段的属性也会认为是映射属性，此时,列名与属性名相同.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AsFieldAttribute:Attribute
    {
        public bool Auto { set; get; }
        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool Primary { set; get; }
        /// <summary>
        /// 映射的到字段的名称
        /// </summary>
        public string FieldName { set; get; }
    }
}
