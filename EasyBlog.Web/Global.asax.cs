using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using EasyBlog.Data;
using EasyBlog.Web.Core;
using Microsoft.Extensions.Configuration;
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
           
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly).InstancePerRequest(); //Register WebApi Controllers
            
            IConfigurationBuilder config = new ConfigurationBuilder(); // Instancia do Microsoft ConfigurationBuilder
            config.AddJsonFile("autofac.json"); // Arquivo que contem os registros de classes

            ConfigurationModule module = new ConfigurationModule(config.Build()); // Instancia e registro da ConfigurationModule

            builder.RegisterModule(module); // Registrando o módulo as coisas que encontrar na configuração para a qual está apontando

            builder.RegisterFilterProvider(); // manipula os registros de quaisquer filtros MVC

            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration); // manipula os registros de quaisquer filtros para API

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container); //Configuracao do container para WebApi

            IExtensibilityManager extensibilityManager = container.Resolve<IExtensibilityManager>(); // resolvendo a partir do container garantindo que o container instancie e persista a classe
            extensibilityManager.GetModuleEvents(); // Chamada para inicializar a variável criada e armazená-la no próprio ExtensibilityManager
        }
    }
}
