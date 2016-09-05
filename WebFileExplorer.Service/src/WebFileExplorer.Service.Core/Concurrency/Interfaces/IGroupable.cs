using System;

namespace WebFileExplorer.Service.Core.Concurrency.Interfaces
{
    /// <summary>
    /// Represents object which contains in group (in any understanding of group) and can be identified by group id
    /// </summary>
    public interface IGroupable
    {
        Guid OperationGroupId { get; }
    }
}