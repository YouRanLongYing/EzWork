using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections;

namespace Ez.DBContract
{
    public class SqlCollection
    {
        public string SQlString { internal set; get; }
        public DbParam[] DbParams { internal set; get; }
        public SqlCollection(string sqlString, params DbParam[] dbParams)
        {
            this.SQlString = sqlString;
            this.DbParams = dbParams;
        }
    }
}
