using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.DBContract;

namespace Ez.DBContract
{
    /// <summary>
    /// 数据库配置信息
    /// </summary>
    public class ConnectionEntity
    {
        public string Scope { private set; get; }
        public DBTypeEnum Dbtype { private set; get; }
        public string ConnectionString { private set; get; }

        public ConnectionEntity(string Scope, DBTypeEnum Dbtype, string ConnectionString)
        {
            this.Scope = Scope;
            this.Dbtype = Dbtype;
            this.ConnectionString = ConnectionString;
        }
    }
}
