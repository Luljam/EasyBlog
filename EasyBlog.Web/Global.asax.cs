using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using EasyBlog.Web.Core;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EasyBlog.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //IExtensibilityManager extensibilityManager = new ExtensibilityManager(); // nao precisa mais pq esta sendo registrada pelo Autofac

            //if (Application["ModuleEvents"] == null)
            //    Application["ModuleEvents"] = extensibilityManager.GetModuleEvents();

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly).InstancePerRequest(); //Register WebApi Controllers

            builder.RegisterType<ExtensibilityManager>().As<IExtensibilityManager>().SingleInstance(); // Registro de classe como singleton

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container); //Configuracao do container para WebApi

            IExtensibilityManager extensibilityManager = container.Resolve<IExtensibilityManager>(); // resolvendo a partir do container garantindo que o container instancie e persista a classe
            extensibilityManager.GetModuleEvents(); // Chamada para inicializar a variável criada e armazená-la no próprio ExtensibilityManager
        }
    }
}
