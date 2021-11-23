using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using EasyBlog.Data;
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

            builder.RegisterAssemblyTypes(typeof(BlogPostRepository).Assembly) // Varre os repositórios
                .Where(t => t.Name.EndsWith("Repository")) // Limita os tipos que terminam com 'Repository'
                .As(t => t.GetInterfaces()?.FirstOrDefault( //Obtendo Associação de interface, o ponto de interrogação serve para se a lista nao for nula
                    i => i.Name == "I" + t.Name)) // Pega a primeira interface  na lista em que o nome é igual do tipo, mas com a letra I na frente
                .InstancePerRequest() // Adiciona a instancia por solicitacao
                .WithParameter(new TypedParameter(typeof(string), "easyBlog")); // e a injeçãop de parametros

            builder.RegisterType<ExtensibilityManager>().As<IExtensibilityManager>().SingleInstance(); // Registro de classe como singleton

            builder.RegisterType<BlogPostRepository>().As<IBlogPostRepository>()
                .WithParameter(new TypedParameter(typeof(string), "easyBlog")); // Informa ao Autofac sempre que encontrar necessidade de resolver a interface IBlogRepository ele encontrará um argumento de string no construtor e use a string easyBlog 
            //.WithParameter(new NamedParameter("connectionStringNamed", "easyBlog")); // também pode ser usado assim

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container); //Configuracao do container para WebApi

            IExtensibilityManager extensibilityManager = container.Resolve<IExtensibilityManager>(); // resolvendo a partir do container garantindo que o container instancie e persista a classe
            extensibilityManager.GetModuleEvents(); // Chamada para inicializar a variável criada e armazená-la no próprio ExtensibilityManager
        }
    }
}
