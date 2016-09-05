using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using WebFileExplorer.Service.REST.Configuration;

namespace WebFileExplorer.Service.REST.AutofacModules
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(CreateRestServiceConfiguration).SingleInstance();
            builder.Register(CreateRequestConfiguration).SingleInstance();
        }

        private static IRestServiceConfiguration CreateRestServiceConfiguration(IComponentContext context)
        {
            var configuration = context.Resolve<IConfiguration>();
            return new RestConfiguration
            {
                Host = configuration["RestModule:Host"],
                Port = configuration["RestModule:Port"],
            };
        }

        private static IRequestConfiguration CreateRequestConfiguration(IComponentContext context)
        {
            var configuration = context.Resolve<IConfiguration>();
            return new RequestConfiguration
            {
                ExecutionTimeout = TimeSpan.Parse(configuration["Request:ExecutionTimeout"])
            };
        }

        #region Nested Class

        private class RestConfiguration : IRestServiceConfiguration
        {
            public string Host { get; set; }
            public string Port { get; set; }
            public Uri Uri => new Uri($"{Host}:{Port}");
        }

        private class RequestConfiguration : IRequestConfiguration
        {
            public TimeSpan ExecutionTimeout { get; set; }
        }

        #endregion
    }
}
