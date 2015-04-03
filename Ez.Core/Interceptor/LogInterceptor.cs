using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using Spring.Aop.Framework.AutoProxy;
using Spring.Aop.Support;
using log4net;
using Spring.Aspects.Logging;

namespace Ez.Core.Interceptor
{
    /// <summary>
    /// 日志拦截器
    /// </summary>
    public class LogInterceptor : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            if (invocation.Method.Name.Equals("Dispose") && invocation.Target is IDisposable)
            {
                return invocation.Proceed();
            }
            else
            {
                LogLevel logLevel = Log4NetManager.DefaultLogger.IsDebugEnabled ? LogLevel.Debug : LogLevel.Info;
                ExecuteInfo exeInfo = new ExecuteInfo(invocation, false, logLevel);
                if (LogLevel.Debug == logLevel)
                {
                    exeInfo.StackTrace = new System.Diagnostics.StackTrace(1, true);
                }
                AsyncOutputDelegate logDelegate = new AsyncOutputDelegate(Log4NetManager.Output);
                logDelegate.BeginInvoke(exeInfo,null,null);
                object result = invocation.Proceed();
                exeInfo.IsEndPoint = true;
                logDelegate.BeginInvoke(exeInfo,null,null);
                return result;
            }
        }
    }
}
