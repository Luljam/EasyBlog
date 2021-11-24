using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBlog.Web.Core
{
    // Classe tem a função de resolver um componente do container
    // Depois injetar a classe no controller
    public class ComponentLocator : IComponentLocator
    {
        public ComponentLocator(ILifetimeScope container)
        {
            _Container = container;
        }
        ILifetimeScope _Container;

        T IComponentLocator.ResolveComponet<T>()
        {
            return _Container.Resolve<T>();
        }
    }

    public interface IComponentLocator
    {
        T ResolveComponet<T>();
    }
}