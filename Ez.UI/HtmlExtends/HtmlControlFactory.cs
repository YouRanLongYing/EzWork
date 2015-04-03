using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Ez.UI.HtmlExtend;
using System.Reflection;
using Ez.Core;
using System.Web.Mvc.Html;
using System.Runtime.Remoting;
using Ez.UI.HtmlExtends.Controls;
using Ez.Core.Interceptor;
namespace Ez.UI.HtmlExtends
{
    internal class HtmlControlFactory
    {
        private static HtmlControlFactory _Instance;
        public static HtmlControlFactory Instance
        {
            get
            {
                if (_Instance == null) _Instance = new HtmlControlFactory();
                return _Instance;
            }
        }
        public string CreateControlHtmlString(PropUIMetadata metadata, HtmlHelper htmlHelper)
        {
            BaseHtmlControl control = null;
            string typename = "Ez.UI.HtmlExtends.Controls.Html" + metadata.UIType;
            try
            {
                Object instace = Assembly.Load("EzUI").CreateInstance(typename);
                control = instace as BaseHtmlControl;
            }
            catch (Exception exp)
            {
                Log4NetManager.Output(new ExecuteInfo
                {
                    Args = new object[] { "UI", typename },
                    Exception = exp,
                    LogLevel = LogLevel.Error,
                    IsEndPoint = false,
                    MethodName = " Assembly.Load or CreateInstance",
                    TargetType = this.GetType()
                });
            }
            if (control != null)
            {
                control.SetParameter(metadata, htmlHelper);
                return control.CreateHtmlString();
            }
            return string.Empty;
        }
    }
}
