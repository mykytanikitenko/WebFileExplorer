using Autofac;
using WebFileExplorer.Service.Core.Concurrency;
using WebFileExplorer.Service.Core.Concurrency.Interfaces;

namespace WebFileExplorer.Service.REST.AutofacModules
{
    public class RequestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RequestPool>()
                .As<IRequestPool>()
                .SingleInstance();
        }
    }
}
