using System;
using Ez.Dtos.Library;

namespace Ez.Dtos.Entities
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class FW_Sys_Log : BaseEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 日志标题
        /// </summary>
        public string log_title { set; get; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public int log_type { set; get; }
        /// <summary>
        /// 日志描述
        /// </summary>
        public string log_ctn { set; get; }
        /// <summary>
        /// 日志发生时间
        /// </summary>
        public DateTime occure_time { set; get; }
        /// <summary>
        /// 发生的IP
        /// </summary>
        public string occure_ip { set; get; }
        /// <summary>
        /// 发生的Mac
        /// </summary>
        public string occure_mac { set; get; }
        /// <summary>
        /// 操作账户（登录账户）
        /// </summary>
        public int login_id { set; get; }
    }
}
