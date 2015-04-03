using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Ez.UI;
using System.Reflection;
using Ez.Core.Attributes;
using Ez.Config;
using System.Collections;
using Ez.Helper;
using Ez.Core;
namespace Ez.Dtos.Library
{
    /// <summary>
    /// 分页模型
    /// created by kongjing
    /// </summary>
    /// <typeparam name="T">分页数据模型</typeparam>
    [Serializable]
    public class PageDto<T> : DefaultDto
    {
        internal IDictionary<string, object> dic = new Dictionary<string, object>();
        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize = 0;
        public PageDto(int pageSize = 0)
        {
            this.PageSize = pageSize;
            //http://localhost:24101/UCenter/UCenter/RoleViewForAjax?_search=false&nd=1409646465652&rows=10&page=1&sidx=&sord=asc
            this.PageIndex = Tools.RequestQuery("page").ToSafeInt();
            this.PageSize = Tools.RequestQuery("rows").ToSafeInt();
            this.OrderBy = Tools.RequestQuery("sidx");
            this.IsDesc = "desc".Equals(Tools.RequestQuery("sord"));
            if (this.PageSize <= 0)
            {
                this.PageSize = UIConfig.Model.PageSize;
                if (this.PageSize <= 0) this.PageSize = 20;
            }
        }
        public IDictionary<string, string> QueryStrings = new Dictionary<string, string>();
        private int pageIndex;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get {
                if (pageIndex <= 0) pageIndex = 1;
                else if (pageIndex > this.Total && this.Total>0) this.pageIndex = this.Total;
                return pageIndex;
            }
            set { pageIndex = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int Total { get {
            if (this.PageSize == 0) return 0;
            return (this.Records + this.PageSize - 1) / this.PageSize;
        } }
        /// <summary>
        /// 总计录数
        /// </summary>
        public int Records{set;get;}
        /// <summary>
        /// 排序的字段
        /// </summary>
        public string OrderBy { set; get; }
        /// <summary>
        /// 是否倒序排列
        /// </summary>
        public bool IsDesc { set; get; }

        /*{
         * "page":"1",
         * "total":1,
         * "records":"13",
         * "rows":[
         *          {"id":"13","cell":["13","2007-10-06","Client 3","1000.00","0.00","1000.00",null]},
         *          {"id":"12","cell":["12","2007-10-06","Client 2","700.00","140.00","840.00",null]},
         *          {"id":"11","cell":["11","2007-10-06","Client 1","600.00","120.00","720.00",null]},
         *          {"id":"10","cell":["10","2007-10-06","Client 2","100.00","20.00","120.00",null]},
         *          {"id":"9","cell":["9","2007-10-06","Client 1","200.00","40.00","240.00",null]},
         *          {"id":"8","cell":["8","2007-10-06","Client 3","200.00","0.00","200.00",null]},
         *          {"id":"7","cell":["7","2007-10-05","Client 2","120.00","12.00","134.00",null]},
         *          {"id":"6","cell":["6","2007-10-05","Client 1","50.00","10.00","60.00",""]},
         *          {"id":"5","cell":["5","2007-10-05","Client 3","100.00","0.00","100.00","no tax at all"]},
         *          {"id":"4","cell":["4","2007-10-04","Client 3","150.00","0.00","150.00","no tax"]},
         *          {"id":"3","cell":["3","2007-10-02","Client 2","300.00","60.00","360.00","note invoice 3 & and amp test"]},
         *          {"id":"2","cell":["2","2007-10-03","Client 1","200.00","40.00","240.00","note 2"]},
         *          {"id":"1","cell":["1","2007-10-01","Client 1","100.00","20.00","120.00","note 1"]}
         *          ],
         *          "userdata":{"amount":3820,"tax":462,"total":4284,"name":"Totals:"}}
         * */

        public IList<T> Results { set; get; }

        public JsResult AsJsResult(IDictionary<string,object> userdata=null)
        {
                    IList<IDictionary<string, string>> rows = null;
                    if (Results != null)
                    {
                        rows = new List<IDictionary<string, string>>();
                        foreach (T data in Results)
                        {
                            IDictionary<string, string> row = new Dictionary<string, string>();
                            PropertyInfo[] meminfo = data.GetType().GetProperties();
                            foreach (PropertyInfo item in meminfo)
                            {
                                object[] objs = item.GetCustomAttributes(typeof(JsonItemAttribute), false);
                                if (objs == null || objs.Length == 0) continue;
                                JsonItemAttribute cattr = objs[0] as JsonItemAttribute;
                                var value = item.GetValue(data, null) ?? "";
                                //cattr.Key = item.Name;
                                row.Add((string.IsNullOrEmpty(cattr.Key) ? item.Name : cattr.Key), value.ToString());
                            }
                            rows.Add(row);
                        }
                    }
                    
                    if (userdata != null)
                    {
                        dic.Add("userdata", userdata);
                    }
                    dic.Add("page", this.PageIndex);
                    dic.Add("total", this.Total);
                    dic.Add("records", this.Records);
                    dic.Add("rows", rows);
                    return new JsResult(dic);
        }

        public static JsResult NoRightProcess()
        {
            var noright_dto = (new PageDto<T>());
            noright_dto.dic.Add("_sys_logined", false);
            return noright_dto.AsJsResult();
        }
    }
}
