using System;
using Ez.Core.Attributes;
using Ez.Dtos.Library;

namespace Ez.Dtos.Entities
{
    /// <summary>
    /// 部门信息
    /// </summary>
    [Serializable]
    public class FW_Department : BaseEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [JsonItem(Key = "no",PrimaryKey=true)]
        public int id { set; get; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [JsonItem(Key="depname")]
        public string dep_name { set; get; }
        /// <summary>
        /// 部门描述
        /// </summary>
        [JsonItem(Key = "descript")]
        public string dep_desp { set; get; }
        /// <summary>
        /// 上级部门编号
        /// </summary>
        public int dep_parentid { set; get; }
        /// <summary>
        /// 部门状态 1 正常  0 不可用
        /// </summary>
        [JsonItem(Key = "status")]
        public int dep_status { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonItem(Key = "createtime")]
        public DateTime create_time { set; get; }
        /// <summary>
        /// 上级部门路径，
        /// 例如 A部门的上级i部门B 的id为1，B的上级部门C的ID为2
        /// 那么 A部门的父级路径的格式为 “2>1”
        /// 如果为顶级部门则留空 （注意：不是 dbnull）
        /// </summary>
        public string parent_path { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        [JsonItem(Key = "remark")]
        public string remark { set; get; }
    }
}
