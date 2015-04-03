using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using System.Reflection;
using Spring.Aop;

namespace Ez.Core.Interceptor
{
    public class ExceptionInterceptor : IThrowsAdvice
    {
        AsyncOutputDelegate logDelegate = new AsyncOutputDelegate(Log4NetManager.Output);
        public void AfterThrowing(MethodInfo method, object[] args, object target, Exception ex)
        {
            logDelegate.BeginInvoke(new ExecuteInfo(target.GetType(), method, args, LogLevel.Error, false, ex),null,null);


            //IController errorController = new UBIQ.Controllers.Framework.ErrorController();
            //errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
