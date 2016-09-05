using System;

namespace WebFileExplorer.Service.REST.Configuration
{
    public interface IRequestConfiguration
    {
        TimeSpan ExecutionTimeout { get; }
    }
}