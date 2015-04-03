using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UBIQ.Dtos.Framework.Models
{
    public class Frm_MappedUrl
    {
        /// <summary>
        /// Url
        /// </summary>
        public string ognURL { set; get; }
        /// <summary>
        /// 端地址代码
        /// </summary>
        public string shortCode { set; get; }
        /// <summary>
        ///携带的业务数据格式如：gpmx-id,即业务标记-数据,常规的做法一般不需要设置此参数，如有地址带有文件下载信息时才会用到
        /// ：如果使用次参数请自行编写关于此地址的解析逻辑
        /// </summary>
        public string data { set; get; }
    }
}
