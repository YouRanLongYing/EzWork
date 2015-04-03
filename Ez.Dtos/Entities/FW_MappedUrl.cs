using System;
using Ez.Dtos.Library;

namespace Ez.Dtos.Entities
{
    [Serializable]
    public class FW_MappedUrl : BaseEntity
    {
        /// <summary>
        /// 短地址代码
        /// </summary>
        public string short_code { set; get; }
        /// <summary>
        /// Url
        /// </summary>
        public string source_url { set; get; }
        /// <summary>
        ///携带的业务数据格式如：gpmx-id,即业务标记-数据,常规的做法一般不需要设置此参数，如有地址带有文件下载信息时才会用到
        /// ：如果使用次参数请自行编写关于此地址的解析逻辑
        /// </summary>
        public string with_data { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { set; get; }

        /// <summary>
        /// 用户登录的id
        /// </summary>
        public int login_id { set; get; }
    }
}
