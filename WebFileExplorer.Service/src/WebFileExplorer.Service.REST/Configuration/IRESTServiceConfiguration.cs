using System;

namespace WebFileExplorer.Service.REST.Configuration
{
    public interface IRestServiceConfiguration
    {
        string Host { get;}
        string Port { get; }
        Uri Uri { get; }
    }
}
