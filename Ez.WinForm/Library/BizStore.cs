using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ez.BizContract;

namespace Ez.WinForm.Library
{
    public class BizStore
    {
        private static IDictionary<string, object> objects = new Dictionary<string, object>();

        private static IApplicationContext ctx;
        protected static IApplicationContext Ctx {
            get
            {
                if (ctx == null)
                    ctx = ContextRegistry.GetContext();
                return ctx;
            }
        }

        /// <summary>
        /// 账户管理业务
        /// </summary>
        public static IAccountBiz AccountBiz
        {
            get
            {
                if (objects.ContainsKey("AccountBiz") && objects["AccountBiz"]!=null)
                { 
                  return objects["AccountBiz"] as IAccountBiz;
                }
                else
                {
                    IAccountBiz instance = (IAccountBiz)Ctx.GetObject("AccountBiz");//Ez.Core.Utils.GetSpringObject<IAccountBiz>("AccountBiz");
                    objects.Add("AccountBiz", instance);
                    return instance;
                }
            }
        }

        /// <summary>
        /// 账户管理业务
        /// </summary>
        public static ILayoutBiz LayoutBiz
        {
            get
            {
                if (objects.ContainsKey("LayoutBiz") && objects["LayoutBiz"] != null)
                {
                    return objects["LayoutBiz"] as ILayoutBiz;
                }
                else
                {
                    ILayoutBiz instance = (ILayoutBiz)Ctx.GetObject("LayoutBiz");//Ez.Core.Utils.GetSpringObject<IAccountBiz>("AccountBiz");
                    objects.Add("LayoutBiz", instance);
                    return instance;
                }
            }
        }


        public static void Dispose()
        {
            Ctx.Dispose();
        }
    }
}
