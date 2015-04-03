using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.BizContract
{
    /// <summary>
    /// 业务层处理结果
    /// created by kongjing
    /// </summary>
    public class BizResult : BizResult<Object>
    {
        public BizResult(bool success, Object result, string message = ""):base(success,result,message)
        {

        }
        public BizResult()
            : base(false)
        {

        }
        public BizResult(bool success)
            : base(success)
        {

        }
        public BizResult(string message,bool success)
            : base(success, null, message)
        {

        }
    }
    /// <summary>
    /// 业务层处理结果
    /// created by kongjing
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    public class BizResult<T>
    {
        private bool success;
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }
        private T data;
        public T Data
        {
            get { return data; }
            set { data = value; }
        }
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public BizResult() { }
        public BizResult(bool success, T data, string message = "")
        {
            this.success = success;
            this.data = data;
            this.message = message;
        }
        public BizResult(bool success)
        {
            this.success = success;
            this.data = default(T);
            this.message = "";
        }
    }
}
