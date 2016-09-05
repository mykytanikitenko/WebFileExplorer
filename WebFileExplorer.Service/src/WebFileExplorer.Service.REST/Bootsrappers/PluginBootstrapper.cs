using Nancy.Hosting.Self;
using WebFileExplorer.Service.Core.Bootstrapping;
using WebFileExplorer.Service.Core.Logging.Interfaces;
using WebFileExplorer.Service.REST.Configuration;

namespace WebFileExplorer.Service.REST.Bootsrappers
{
    public class PluginBootstrapper : IPluginBootstrapper
    {
        private readonly ILogger logger;
        private readonly NancyHost nancyHost;
        private readonly IRestServiceConfiguration serviceConfiguration;

        public PluginBootstrapper(
            ILogger logger,
            NancyHost nancyHost,
            IRestServiceConfiguration serviceConfiguration)
        {
            this.logger = logger;
            this.nancyHost = nancyHost;
            this.serviceConfiguration = serviceConfiguration;
        }

        public void Stop()
        {
            logger.LogOk("REST service stopped");
            nancyHost.Stop();

            logger.LogOk("REST module stopping");
        }

        public void Start()
        {
            logger.LogOk("REST module started");

            nancyHost.Start();
            logger.LogOk("REST service started on " + serviceConfiguration.Uri.AbsoluteUri);
        }
    }
}
