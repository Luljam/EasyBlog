using Autofac;
using EasyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBlog.Web.Core
{
    public class RepositoryRegistrationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogPostRepository>().As<IBlogPostRepository>()
               .WithParameter(new TypedParameter(typeof(string), "easyBlog")); // Informa ao Autofac sempre que encontrar necessidade de resolver a interface IBlogRepository ele encontrará um argumento de string no construtor e use a string easyBlog 
            //.WithParameter(new NamedParameter("connectionStringNamed", "easyBlog")); // também pode ser usado assim

            builder.RegisterAssemblyTypes(typeof(BlogPostRepository).Assembly) // Varre os repositórios
                .Where(t => t.Name.EndsWith("Repository")) // Limita os tipos que terminam com 'Repository'
                .As(t => t.GetInterfaces()?.FirstOrDefault( //Obtendo Associação de interface, o ponto de interrogação serve para se a lista nao for nula
                    i => i.Name == "I" + t.Name)) // Pega a primeira interface  na lista em que o nome é igual do tipo, mas com a letra I na frente
                .InstancePerRequest() // Adiciona a instancia por solicitacao
                .WithParameter(new TypedParameter(typeof(string), "easyBlog")); // e a injeçãop de parametros
        }
    }
}