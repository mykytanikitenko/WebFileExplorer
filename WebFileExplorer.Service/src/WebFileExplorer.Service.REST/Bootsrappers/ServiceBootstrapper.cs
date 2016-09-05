using Autofac;
using Nancy.Bootstrappers.Autofac;

namespace WebFileExplorer.Service.REST.Bootsrappers
{
    internal class ServiceBootstrapper : AutofacNancyBootstrapper
    {
        private readonly ILifetimeScope lifetimeScope;

        public ServiceBootstrapper(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        protected override ILifetimeScope GetApplicationContainer()
        {
            return lifetimeScope;
        }
    }
}
