using System.Collections.Generic;
using System.Threading;
using WebFileExplorer.ServiceDomain.Domain;

namespace WebFileExplorer.Service.FileSystem.Services.Interfaces
{
    public interface IFileExplorerService
    {
        CountFilesResult CountFilesInDirectory(string path, CancellationToken cancellationToken);
        IEnumerable<FileSystemItem> GetItemsInPath(string path, CancellationToken cancellationToken);
        IEnumerable<FileSystemItem> GetDrives(CancellationToken cancellationToken);
    }
}
