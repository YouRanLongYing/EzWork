using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Dtos.Library;

namespace Ez.Dtos
{
    public class ErrorDto:BaseDto
    {
        public ErrorDto(Exception exception, string rawUrl, bool debug)
        {
            this.Exception = exception;
            this.RawUrl = rawUrl;
            this.Debug = debug;
        }
        public Exception Exception { private set; get; }
        public string RawUrl { private set; get; }
        public bool Debug { private set; get; }
        public object CustomData { set; get; }
    }
}
