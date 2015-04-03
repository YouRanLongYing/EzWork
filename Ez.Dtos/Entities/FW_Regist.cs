using System;
using Ez.Dtos.Library;

namespace Ez.Dtos.Entities
{
    /// <summary>
    /// 系统注册
    /// </summary>
    [Serializable]
    public class FW_Regist : BaseEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public int sys_name { set; get; }
        /// <summary>
        /// 系统描述
        /// </summary>
        public int sys_description { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { set; get; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public int secret_key { set; get; }
        /// <summary>
        /// 系统版本
        /// </summary>
        public int sys_version { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public int create_time { set; get; }
        /// <summary>
        /// 项目经理
        /// </summary>
        public int project_magr { set; get; }
        /// <summary>
        /// 项目成员
        /// </summary>
        public int proj_member { set; get; }
        /// <summary>
        /// 授权给
        /// </summary>
        public int licensedto { set; get; }
        /// <summary>
        /// 语种
        /// </summary>
        public int lang { set; get; }
    }
}
