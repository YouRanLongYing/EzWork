using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.DBContract
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBTypeEnum
    {
        /// <summary>
        /// Access数据库
        /// </summary>
        Access,
        /// <summary>
        /// SQlServer数据库
        /// </summary>
        MSSQL,
        /// <summary>
        /// Oracle数据库
        /// </summary>
        Oracle,
        /// <summary>
        /// mysql数据库
        /// </summary>
        MySql
    }
}
