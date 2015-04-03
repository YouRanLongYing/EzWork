using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ez.UI;
using Spring.Web.Mvc;
using Ez.UI.Adapters;
using Ez.Lang;
using System.ComponentModel.DataAnnotations;
using Ez.UI.Attributes;
using Ez.UI.ViewExtend;
using Ez.Config;

namespace Ez.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    /// <summary>
    /// 框架应用程序
    /// </summary>
    public class MvcApplication : SpringMvcApplication//System.Web.HttpApplication
    {
        /// <summary>
        /// 注册框架的全局过滤器
        /// </summary>
        /// <param name="filters">全局过滤器集合</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CultureInfoAttribute());
            filters.Add(new LayoutAttribute());
            filters.Add(new HandleErrorAttribute());
        }
        /// <summary>
        /// 注册框架路由
        /// </summary>
        /// <param name="routes">框架路由集合</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "url", // Route name
                "url/{code}", // URL with parameters// UIConfig.Model.LayoutAction
                new { controller = "url", action = "index", code = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters// UIConfig.Model.LayoutAction
                new { controller = UIConfig.Model.CtrlDir, action ="index" , id = UrlParameter.Optional } // Parameter defaults
            );//UIConfig.Model.LayoutAction
        }
        /// <summary>
        /// 启动框架应用
        /// </summary>
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CRazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            log4net.Config.XmlConfigurator.Configure();
            RegisterGlobalFilters(GlobalFilters.Filters);

            CRouter.MapRouterRules(RouteTable.Routes);
            RegisterRoutes(RouteTable.Routes);
            RouteTable.Routes.RouteExistingFiles = false;

            ModelMetadataProviders.Current = new DisplayNameMetadataProvider(EzLanguage.ResourceManager);
            DataAnnotationsModelValidatorProvider.RegisterAdapterFactory(typeof(RequiredAttribute),
                (metadata, controllerContext, attribute)
                 => new CRequiredAttributeAdapter(metadata, controllerContext, (RequiredAttribute)attribute));


            var clientDataTypeValidator = ModelValidatorProviders.Providers.OfType<ClientDataTypeModelValidatorProvider>().FirstOrDefault();
            if (null != clientDataTypeValidator)
            {
                ModelValidatorProviders.Providers.Remove(clientDataTypeValidator);
            }
            //ModelValidatorProviders.Providers.Add(new FilterableClientDataTypeModelValidatorProvider());
            //EzLanguage.Provider = EzLanguage.ResourceManager;
        }
    }
    /// <summary>
    /// 框架提供给产品程序的应用程序接口用于启动产品应用程序 来注入产品多语言信息
    /// </summary>
    public class FrameworkApplication : MvcApplication
    {
        private static FrameworkApplication instance;
        /// <summary>
        /// 启动框架应用
        /// </summary>
        /// <param name="provider">产品多语言支持接口</param>
        public void AppStart(LanguageAssemblyProvider provider)
        {
            EzLanguage.AssemblyProvider = provider;
            this.Application_Start();
        }
        /// <summary>
        /// 单例模式获取框架应用实例
        /// </summary>
        public static FrameworkApplication Instance
        {

            get
            {
                if (instance == null) instance = new FrameworkApplication();
                return instance;
            }
        }
    }
}