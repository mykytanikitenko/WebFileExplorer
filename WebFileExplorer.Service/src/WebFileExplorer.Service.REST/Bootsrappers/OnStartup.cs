using Nancy.Bootstrapper;
using Nancy.Extensions;
using WebFileExplorer.Service.Core.Logging.Interfaces;

namespace WebFileExplorer.Service.REST.Bootsrappers
{
    public class OnStartup : IApplicationStartup
    {
        private readonly ILogger logger;

        public OnStartup(ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines
                .BeforeRequest
                .AddItemToStartOfPipeline(ctx => {
                    if (ctx != null)
                    {
                        logger.LogDebug($"Request [{ctx.Request.Method}] {ctx.Request.Url}\n{ctx.Request.Body.AsString()}");
                        ctx.Request.Body.Position = 0;
                    }

                return null;
            });

            pipelines
                .OnError
                .AddItemToStartOfPipeline((ctx, ex) => {
                    if (ctx != null)
                        logger.LogError($"Request [{ctx.Request.Method}] {ctx.Request.Url}\n {ex.Message} \n{ex.StackTrace}");

                    return null;
                });
        }
    }
}
