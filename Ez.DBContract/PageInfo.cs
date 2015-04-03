using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Collections;

namespace Ez.DBContract
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo : SqlCollection
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        /// <param name="pageSzie">页容量</param>
        /// <param name="sql">执行SQL</param>
        /// <param name="dbParams">SQL参数</param>
        public PageInfo(int pageSzie, string sql, params DbParam[] dbParams)
            : base(sql, dbParams)
        {
            this.PageSize = pageSzie;
        }
        /// <summary>
        /// 记录数
        /// </summary>
        public int Total { set; get; }


        private int pageSize;
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize
        {
            get
            {
                if (this.pageSize <= 0)
                {
                    this.pageSize = 20;
                }
                return pageSize;
            }
            set { pageSize = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return (this.Total + this.PageSize-1) / this.PageSize;
            }
        }

        private int pageIndex;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (pageIndex <= 0) pageIndex = 1;
                return pageIndex;
            }
            set { pageIndex = value; }
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        public DataSet PageData { set; get; }
        /// <summary>
        /// 分页发生的异常
        /// </summary>
        public Exception Exception { set; get; }
    }
    /// <summary>
    /// 分页查询的SQL模型
    /// </summary>
    public class QuerySql
    {
        /// <summary>
        /// 唯一索引 用于分页排序，一般为ID
        /// </summary>
        public string SequenceColumnName { set; get; }
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize{set;get;}
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { set; get; }
        /// <summary>
        /// 查询的字列
        /// </summary>
        public string SelectColumns{set;get;}
        /// <summary>
        /// 数据来源的表
        /// </summary>
        public string FromTableNames { set; get; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public IDictionary<string,string> EqualCondition { set; get; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public IDictionary<string, string> LikeCondition { set; get; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy { set; get; }
        /// <summary>
        /// 分组字段
        /// </summary>
        public string GroupBy { set; get; }
        /// <summary>
        /// 分组筛选条件
        /// </summary>
        public string Having { set; get; }
        /// <summary>
        /// 是否倒序
        /// </summary>
        public bool IsDesc { set; get; }

        /// <summary>
        /// 生成查询的SQL
        /// </summary>
        /// <returns></returns>
        public string ToString(DBTypeEnum dbtype)
        {
            string orderString = "";
            string selectMaxFlag = "";
            string pageflag = this.SequenceColumnName;
            if (!string.IsNullOrEmpty(this.OrderBy))
            {
                string[] fileds = this.OrderBy.Split(',');
                IList<string> files_list = new List<string>();
                if (this.OrderBy.Contains(this.SequenceColumnName))
                {
                    foreach (string filed in fileds)
                    {
                        if (filed.Equals(this.SequenceColumnName)) continue;
                        files_list.Add(filed);
                    }
                    this.OrderBy = string.Join(",", files_list.ToArray());
                }
                if (files_list.Count > 0)
                {
                    pageflag = files_list.FirstOrDefault();
                }
                else
                {
                    pageflag = fileds.FirstOrDefault();
                }
                orderString = "order by " + this.OrderBy.TrimStart(',') + " " + (this.IsDesc ? "desc" : "asc");
            }
            else
            {
                orderString = "order by " + this.SequenceColumnName + " " + (this.IsDesc ? "desc" : "asc");
            }

            int selectedtop = (this.PageIndex - 1) * this.PageSize;
            if (selectedtop > 0)
            {
                selectMaxFlag = string.Format("(select {4}({0}) from (select top {1} {0} from {2} {3}) as MaxFlags)",
                    pageflag,
                    selectedtop,
                    this.FromTableNames,
                    orderString,this.IsDesc?"min":"max");
            }
            StringBuilder sqlappend = new StringBuilder("select "+(dbtype== DBTypeEnum.MySql?"":("top "+ this.PageSize + " ")));
            sqlappend.Append(this.SelectColumns + " from ");
            sqlappend.Append(this.FromTableNames + " ");
            StringBuilder count_condition = new StringBuilder();


            string condition = "";
            //后期可能需要优化
            IList<string> conditions = new List<string>();
            if (this.EqualCondition != null && this.EqualCondition.Count > 0)
            {
                foreach (var filed in this.EqualCondition.Keys)
                {
                    conditions.Add(string.Format("{0} = '{1}'", filed, this.EqualCondition[filed]));
                }
            }
            if (this.LikeCondition != null && this.LikeCondition.Count > 0)
            {
                foreach (var filed in this.LikeCondition.Keys)
                {
                    conditions.Add(string.Format("{0} like '%{1}%'", filed, this.LikeCondition[filed]));
                }
            }
            condition = string.Join(" and ",conditions.ToArray());
            if (!string.IsNullOrEmpty(condition))
            {
                count_condition.Append(!string.IsNullOrEmpty(condition) ? condition : "");
            }
            if (selectedtop > 0)
            {
                sqlappend.AppendFormat(" where {0} {1} {2} {3} ", pageflag, this.IsDesc ? "<" : ">", selectMaxFlag, (count_condition.Length > 0 ? " and " + count_condition : ""));
            }
            else if (count_condition.Length>0)
            {
                sqlappend.AppendFormat(" where {0} ", count_condition);
            }

            if (!string.IsNullOrEmpty(this.GroupBy))
            {
                count_condition.Append(this.GroupBy + " ");
                sqlappend.Append(this.GroupBy + " ");
                if (!string.IsNullOrEmpty(this.Having))
                {
                    count_condition.Append(this.Having + " ");
                    sqlappend.Append(this.Having + " ");
                }
            }
            if (!string.IsNullOrEmpty(orderString))
            {
                sqlappend.Append(orderString + " ");
            }
            string countSql = ";select count(1) from " + this.FromTableNames + " " + (count_condition.Length > 0 ? "where " + count_condition : "");
            if (dbtype == DBTypeEnum.MySql)
            {
                sqlappend.Append(" limit 0," + this.PageSize);
            }
            sqlappend.Append(countSql);
            return sqlappend.ToString();
        }
    }
}
