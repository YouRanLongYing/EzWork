using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context.Support;
using Spring.Context;

namespace Ez.Core
{
    public class Utils
    {
        static  IApplicationContext ctx = ContextRegistry.GetContext();
        /// <summary>
        /// 获取Spring.net 容器中的对象
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="objName">实现的对象名</param>
        /// <returns>T类型的实例</returns>
        public static T GetSpringObject<T>(string objName) where T:class
        {
            
            T instance = (T)ctx.GetObject(objName);
            return instance;
        }
    }
}
