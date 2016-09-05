using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebFileExplorer.Service.Core.Concurrency.Interfaces;
using WebFileExplorer.Service.Core.Logging.Interfaces;

namespace WebFileExplorer.Service.Core.Concurrency
{
    /// <summary>
    /// Don't forget specify lifetime scope. Because this object have to be a singleton for particulary scope or for entire application
    /// </summary>
    public class RequestPool : IRequestPool
    {
        private readonly ILogger logger;
        // dotnet concurrency can't in concurrable list and concurrency bag can't remove objects
        // that's why I need this stupid dictionary! byte value doesn't means anything
        private readonly ConcurrentDictionary<ISourcable, byte> operations;

        public RequestPool(ILogger logger)
        {
            this.logger = logger;
            operations = new ConcurrentDictionary<ISourcable, byte>();
        }

        public void CancellAllFromSource(Guid source)
        {
            ClearCompleted();

            var operationsFromOneSource = operations.Keys.Where(s => source == s.Source);
            CancelOperations(operationsFromOneSource);
        }

        public void CancellAllFromSourceInGroup(Guid source, Guid groupId)
        {
            ClearCompleted();

            var operationsFromGroup = operations.Keys.Where(OperationFromSourceAndBelongsToGroup(source, groupId));
            CancelOperations(operationsFromGroup);
        }

        private void CancelOperations(IEnumerable<ISourcable> operationsFromOneSource)
        {
            foreach (var operation in operationsFromOneSource)
            {
                logger.LogDebug($"Cancelling operation from source {operation.Source}");
                ((ICancellable) operation).Cancel();

                RemoveOperation(operation);
            }
        }

        public void Add<TResponse>(ICancellableOperation<TResponse> operation)
        {
            ClearCompleted();

            operations.TryAdd((ISourcable) operation, 0);
        }

        private void RemoveOperation(ISourcable operation)
        {
            byte anyValue; // yes, I hated it stupid variablee too
            operations.TryRemove(operation, out anyValue);
        }

        private static Func<ISourcable, bool> OperationFromSourceAndBelongsToGroup(Guid source, Guid groupId)
        {
            return s => source == s.Source && ((IGroupable) s).OperationGroupId == groupId;
        }

        private void ClearCompleted()
        {
            var completedOperation = operations.Keys.Where(operation => ((ICompletable) operation).IsComplete);
            foreach (var operation in completedOperation)
                RemoveOperation(operation);
        }
    }
}
