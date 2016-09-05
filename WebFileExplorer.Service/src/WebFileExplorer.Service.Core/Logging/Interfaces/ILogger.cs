namespace WebFileExplorer.Service.Core.Logging.Interfaces
{
    public interface ILogger
    {
        void LogOk(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
