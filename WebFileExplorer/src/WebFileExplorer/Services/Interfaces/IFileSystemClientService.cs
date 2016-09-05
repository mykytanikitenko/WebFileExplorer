using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFileExplorer.ApiDomain.FileExplorer.Responses;
using WebFileExplorer.ServiceDomain.Api.Responses;

namespace WebFileExplorer.Services.Interfaces
{
    public interface IFileSystemClientService
    {
        Task<IEnumerable<FileSystemItemResponse>> RequestDrivesAsync(Guid appToken, bool cached = true);
        Task<IEnumerable<FileSystemItemResponse>> RequestEntriesAsync(string path, Guid appToken, bool cached = true);
        Task<IEnumerable<FileSystemItemResponse>> RequstEntiesInParentAsync(string path, Guid appToken, bool cached = true);
        Task<CountFilesResponse> RequestCountFilesAsync(string path, Guid appToken, bool cached = true);
        Task<CountFilesResponse> RequestParentCountFilesAsync(string path, Guid appToken, bool cached = true);
    }
}
