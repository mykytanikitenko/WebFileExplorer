using System;

namespace WebFileExplorer.Service.Core.Concurrency.Interfaces
{
    /// <summary>
    /// Represents something what have Guid source property
    /// </summary>
    public interface ISourcable
    {
        Guid Source { get; }
    }
}
