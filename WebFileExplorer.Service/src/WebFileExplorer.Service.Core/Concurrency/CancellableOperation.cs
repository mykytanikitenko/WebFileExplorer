using System;
using System.Threading;
using System.Threading.Tasks;
using WebFileExplorer.Service.Core.Concurrency.Interfaces;

namespace WebFileExplorer.Service.Core.Concurrency
{
    /// <summary>
    /// Wraps task which have cancellation token and can be grouped
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class CancellableOperation<TRequest,TResponse> : ISourcable, ICompletable, ICancellableOperation<TResponse>
    {
        public Task<TResponse> Operation { get; }
        public Guid Source { get; }
        public bool Cancelled => cancellationTokenSource.IsCancellationRequested;
        public Guid OperationGroupId { get; }
        public bool IsComplete => Operation.IsCompleted;

        private readonly CancellationTokenSource cancellationTokenSource;

        public CancellableOperation(Func<TRequest, CancellationToken, TResponse> action, TRequest request, Guid source, Guid operationGroupId)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Operation = CreateCancellableTaskWithRequest(action, request);
            Source = source;
            OperationGroupId = operationGroupId;
        }

        public CancellableOperation(Func<CancellationToken, TResponse> action, Guid source, Guid operationGroupId)
        {
            cancellationTokenSource = new CancellationTokenSource();
            Operation = CreateCancellableTask(action);
            Source = source;
            OperationGroupId = operationGroupId;
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }

        public void CancelAfter(TimeSpan time)
        {
            cancellationTokenSource.CancelAfter(time);
        }

        private Task<TResponse> CreateCancellableTaskWithRequest(Func<TRequest, CancellationToken, TResponse> task, TRequest request)
        {
            return Task.Factory.StartNew(state => task(request, (CancellationToken) state), cancellationTokenSource.Token);
        }

        private Task<TResponse> CreateCancellableTask(Func<CancellationToken, TResponse> task)
        {
            return Task.Factory.StartNew(state => task((CancellationToken) state), cancellationTokenSource.Token);
        }
    }
}
