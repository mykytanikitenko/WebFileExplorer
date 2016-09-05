using System;

namespace WebFileExplorer.Service.Core.Concurrency.Interfaces
{
    public interface IRequestPool
    {
        void CancellAllFromSource(Guid source);
        void CancellAllFromSourceInGroup(Guid source, Guid groupId);
        void Add<TResponse>(ICancellableOperation<TResponse> operation);
    }
}
