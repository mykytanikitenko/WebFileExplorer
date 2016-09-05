using Autofac;
using WebFileExplorer.Service.Core.Bootstrapping;
using WebFileExplorer.Service.REST.Bootsrappers;

namespace WebFileExplorer.Service.REST.AutofacModules
{
    public class RestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<PluginBootstrapper>()
                .As<IPluginBootstrapper>()
                .SingleInstance();

            builder
                .RegisterType<ServiceBootstrapper>();
        }
    }
}
