using Autofac;
using WebFileExplorer.Services;
using WebFileExplorer.Services.Interfaces;

namespace WebFileExplorer.Autofac.Modules
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<FileSystemClientService>()
                .As<IFileSystemClientService>();
        }
    }
}
