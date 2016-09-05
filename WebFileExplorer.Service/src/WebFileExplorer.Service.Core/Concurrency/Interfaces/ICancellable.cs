using System;

namespace WebFileExplorer.Service.Core.Concurrency.Interfaces
{
    public interface ICancellable
    {
        /// <summary>
        /// Sends information for task for cancellation
        /// </summary>
        void Cancel();

        /// <summary>
        /// Sends infromation for cancellation after time duration
        /// </summary>
        /// <param name="time"></param>
        void CancelAfter(TimeSpan time);
    }
}
