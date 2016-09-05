using System.Threading.Tasks;

namespace WebFileExplorer.Service.Core.Concurrency.Interfaces
{
    /// <summary>
    /// Reresents cancellable operation
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface ICancellableOperation<TResponse> : ICancellable, IGroupable
    {
        Task<TResponse> Operation { get; }
        bool Cancelled { get; }
    }
}
