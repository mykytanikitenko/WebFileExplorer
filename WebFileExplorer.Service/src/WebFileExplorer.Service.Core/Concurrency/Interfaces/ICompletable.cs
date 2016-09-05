namespace WebFileExplorer.Service.Core.Concurrency.Interfaces
{
    public interface ICompletable
    {
        bool IsComplete { get; }
    }
}