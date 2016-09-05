using Autofac;
using WebFileExplorer.Service.REST.Services;
using WebFileExplorer.Service.REST.Services.Interfaces;

namespace WebFileExplorer.Service.REST.AutofacModules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RestFileExplorerService>()
                .As<IRestFileExplorerService>();
        }
    }
}
