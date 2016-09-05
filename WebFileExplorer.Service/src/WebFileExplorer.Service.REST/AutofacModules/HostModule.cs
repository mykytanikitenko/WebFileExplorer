using Autofac;
using Nancy.Hosting.Self;
using WebFileExplorer.Service.REST.Bootsrappers;
using WebFileExplorer.Service.REST.Configuration;

namespace WebFileExplorer.Service.REST.AutofacModules
{
    public class HostModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(CreateNancyHost);
        }

        private static NancyHost CreateNancyHost(IComponentContext context)
        {
            var configuration = context.Resolve<IRestServiceConfiguration>();
            var boostrapper = context.Resolve<ServiceBootstrapper>();

            var hostConfigs = new HostConfiguration
            {
                UrlReservations = new UrlReservations
                {
                    CreateAutomatically = true
                }
            };

            return new NancyHost(configuration.Uri, boostrapper, hostConfigs);
        }
    }
}
