using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.DBContract
{
    /// <summary>
    /// 框架提供的数据计数器表，可用于生成表唯一索引的值
    /// </summary>
    public class FW_Squence
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public int pro_id { set; get; }
        /// <summary>
        /// 与数据库表明不重复的键，可以为表名（计数表数据）
        /// </summary>
        public string skey { set; get; }
        /// <summary>
        /// 序列前缀
        /// </summary>
        public string prefix { set; get; }
        /// <summary>
        /// 当前序号值
        /// </summary>
        public int svalue { set; get; }
        /// <summary>
        /// 产生序号的最小长度
        /// </summary>
        public int min_len { set; get; }
        /// <summary>
        /// 小于指定长度需要补齐的字符
        /// </summary>
        public string supple_char { set; get; }
        /// <summary>
        /// 如果不能满足需求，可定义生成序列的实现
        /// </summary>
        public string ext_realize_dll { set; get; }
    }
}
