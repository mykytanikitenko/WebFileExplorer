using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using WebFileExplorer.Configuration;

namespace WebFileExplorer.Autofac.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(CreateRestServiceConfiguration);
        }

        private static IRestServiceConfiguration CreateRestServiceConfiguration(IComponentContext context)
        {
            var configuration = context.Resolve<IConfiguration>();
            var host = configuration["ApplicationRestService:Host"];
            var port = configuration["ApplicationRestService:Port"];

            return new RestConfiguration
            {
                BaseUri = new Uri($"{host}:{port}")
            };
        }

        #region Nested Class

        private class RestConfiguration : IRestServiceConfiguration
        {
            public Uri BaseUri { get; set; }
        }

        #endregion
    }
}
