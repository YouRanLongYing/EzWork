using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBIQ.Framework.Dtos.Library;

namespace UBIQ.Tpl.Dto.Entity
{
    /// <summary>
    /// 示例
    /// created by kongjing on 2014.12.15
    /// </summary>
    public class Demo:DefaultDto
    {
        /// <summary>
        /// key
        /// created by kongjing on 2014.12.15
        /// </summary>
        public string Key { set; get; }
        /// <summary>
        /// value
        /// created by kongjing on 2014.12.15
        /// </summary>
        public string Value { set; get; }
    }
}
