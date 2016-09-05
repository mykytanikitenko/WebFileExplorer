using System.Collections.Generic;
using Autofac;
using WebFileExplorer.Caching.Repositories;
using WebFileExplorer.Caching.Repositories.Interfaces;
using WebFileExplorer.ServiceDomain.Api.Responses;
using WebFileExplorer.ServiceDomain.Domain;

namespace WebFileExplorer.Service.REST.AutofacModules
{
    public class CacheModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<CacheRepository<string, CountFilesResponse>>()
                .As<ICacheRepository<string, CountFilesResponse>>()
                .SingleInstance();

            builder
                .RegisterType<CacheRepository<string, IEnumerable<FileSystemItem>>>()
                .As<ICacheRepository<string, IEnumerable<FileSystemItem>>>()
                .SingleInstance();
        }
    }
}
