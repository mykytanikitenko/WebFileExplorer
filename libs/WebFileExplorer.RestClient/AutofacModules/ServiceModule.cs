using Autofac;
using WebFileExplorer.RestClient.Services;
using WebFileExplorer.RestClient.Services.Interfaces;

namespace WebFileExplorer.RestClient.AutofacModules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RestClientService>()
                .As<IRestClientService>();
        }
    }
}
