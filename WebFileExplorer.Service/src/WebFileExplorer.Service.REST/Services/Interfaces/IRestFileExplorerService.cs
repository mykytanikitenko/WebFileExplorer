using System.Collections.Generic;
using WebFileExplorer.ServiceDomain.Api.Requests;
using WebFileExplorer.ServiceDomain.Api.Responses;
using WebFileExplorer.ServiceDomain.Domain;

namespace WebFileExplorer.Service.REST.Services.Interfaces
{
    public interface IRestFileExplorerService
    {
        CountFilesResponse CountFiles(FileSystemRequest request);
        IEnumerable<FileSystemItem> GetFileSystemEntries(FileSystemRequest request);
        IEnumerable<FileSystemItem> GetDrives(FileSystemRequest request);
    }
}
