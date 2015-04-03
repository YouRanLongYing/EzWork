using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Core.Attributes
{
    /// <summary>
    /// ORM 映射类为数据库的表，未指定则使用类名作为表名使用（请确保类名与表名一致否则会报错）
    /// 注意：如果想忽略某个属性请在属性添加特性IgnoreField，否则即使没有标明为映射字段的属性也会认为是映射属性，此时,列名与属性名相同.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public class AsTableAttribute:Attribute
    {
        /// <summary>
        /// 映射的到表的名称
        /// </summary>
        public string TableName { set; get; }
    }
}
