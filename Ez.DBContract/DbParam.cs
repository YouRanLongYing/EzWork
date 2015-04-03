using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace Ez.DBContract
{
    public class DbParam
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParameterName { set; get; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { set; get; }

        private ParameterDirection direction = ParameterDirection.Input;

        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// 构造一个参数表述信息
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        public DbParam(string paramName, object value)
        {
            this.ParameterName = paramName;
            this.Value = value;
        }

    }
}
