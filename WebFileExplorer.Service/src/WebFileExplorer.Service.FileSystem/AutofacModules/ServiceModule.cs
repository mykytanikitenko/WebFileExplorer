using Autofac;
using WebFileExplorer.Service.FileSystem.Services;
using WebFileExplorer.Service.FileSystem.Services.Interfaces;

namespace WebFileExplorer.Service.FileSystem.AutofacModules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<FileExplorerService>()
                .As<IFileExplorerService>();
        }
    }
}
