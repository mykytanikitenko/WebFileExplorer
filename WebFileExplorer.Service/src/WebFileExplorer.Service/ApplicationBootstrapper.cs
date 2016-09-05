using Autofac;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using WebFileExplorer.Service.Core.Logging.Interfaces;
using WebFileExplorer.Service.Core.Logging;
using WebFileExplorer.Service.Core.Bootstrapping;

namespace WebFileExplorer.Service
{
    public class ApplicationBootstrapper
    {
        private IContainer container;
        private IEnumerable<IPluginBootstrapper> plugins;
        private readonly ILogger logger;

        public ApplicationBootstrapper()
        {
            logger = GetLogger();
            Bootstrap();
        }

        public void Run()
        {
            logger.LogOk("WebFile.Service started");
            plugins = container.Resolve<IEnumerable<IPluginBootstrapper>>();
            foreach (var plugin in plugins)
                plugin.Start();
        }

        public void Stop()
        {
            foreach (var plugin in plugins)
                plugin.Stop();
        }

        private void Bootstrap()
        {
            var builder = CreateContainer();
            builder.Register(context => GetConfiguration())
                   .SingleInstance();

            builder.Register(context => logger)
                   .SingleInstance();

            container = builder.Build();
        }

        private static Assembly[] GetRuntimeAssemblies()
        {
            var runtime = DependencyContext.Default.Target.Runtime;
            return DependencyContext
                .Default
                .GetRuntimeAssemblyNames(runtime)
                .Where(assembly => assembly.Name.StartsWith("WebFileExplorer"))
                .Distinct()
                .Select(Assembly.Load)
                .ToArray();
        }

        private static ContainerBuilder CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(GetRuntimeAssemblies());

            return containerBuilder;
        }

        private static IConfiguration GetConfiguration()
        {
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            return new ConfigurationBuilder()
                .SetBasePath(applicationBasePath)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static ILogger GetLogger()
        {
            return new SampleConsoleLogger();
        }
    }
}
