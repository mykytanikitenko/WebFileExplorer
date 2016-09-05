using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebFileExplorer.ApiDomain.FileExplorer.Responses;
using WebFileExplorer.Configuration;
using WebFileExplorer.RestClient.Services.Interfaces;
using WebFileExplorer.ServiceDomain.Api.Requests;
using WebFileExplorer.ServiceDomain.Api.Responses;
using WebFileExplorer.ServiceDomain.Domain;
using WebFileExplorer.Services.Interfaces;

namespace WebFileExplorer.Services
{
    internal class FileSystemClientService : IFileSystemClientService
    {
        private readonly IRestClientService restClientService;
        private readonly IRestServiceConfiguration restServiceConfiguration;

        public FileSystemClientService(IRestClientService restClientService, IRestServiceConfiguration restServiceConfiguration)
        {
            this.restClientService = restClientService;
            this.restServiceConfiguration = restServiceConfiguration;
        }

        public async Task<IEnumerable<FileSystemItemResponse>> RequestEntriesAsync(string path, Guid appToken, bool cached = true)
        {
            var request = CreateFileSystemRequest(path, appToken, cached);
            var url = new Uri(restServiceConfiguration.BaseUri, "FileSystem/Entries").AbsoluteUri;

            var response = await restClientService.PostRequestAsync<FileSystemRequest, IEnumerable<FileSystemItem>>(
                url, request);

            return response?.Select(CreateFileSystemItemResponse);
        }

        public Task<IEnumerable<FileSystemItemResponse>> RequstEntiesInParentAsync(string path, Guid appToken, bool cached = true)
        {
            path = GetParentLocation(path);
            return RequestEntriesAsync(path, appToken, cached);
        }

        public async Task<IEnumerable<FileSystemItemResponse>> RequestDrivesAsync(Guid appToken, bool cached = true)
        {
            var request = CreateFileSystemRequest(String.Empty, appToken, cached);
            var url = new Uri(restServiceConfiguration.BaseUri, "FileSystem/Drives").AbsoluteUri;

            var response = await restClientService.PostRequestAsync<FileSystemRequest, IEnumerable<FileSystemItem>>(
                url, request);

            return response?.Select(CreateFileSystemItemResponse);
        }

        public async Task<CountFilesResponse> RequestCountFilesAsync(string path, Guid appToken, bool cached = true)
        {
            var request = CreateFileSystemRequest(path, appToken, cached);
            var url = new Uri(restServiceConfiguration.BaseUri, "FileSystem/Count").AbsoluteUri;

            return await restClientService.PostRequestAsync<FileSystemRequest, CountFilesResponse>(
                url, request);
        }

        public Task<CountFilesResponse> RequestParentCountFilesAsync(string path, Guid appToken, bool cached = true)
        {
            path = GetParentLocation(path);
            return RequestCountFilesAsync(path, appToken, cached);
        }

        private static FileSystemRequest CreateFileSystemRequest(string path, Guid appToken, bool cached)
        {
            return new FileSystemRequest
            {
                Cached = cached,
                Path = path,
                SourceToken = appToken
            };
        }

        private static FileSystemItemResponse CreateFileSystemItemResponse(FileSystemItem fileSystemItem)
        {
            return new FileSystemItemResponse
            {
                Name = fileSystemItem.Name,
                Location = fileSystemItem.Location,
                TypeKey = fileSystemItem.Type.ToString()
            };
        }

        private static string GetParentLocation(string path)
        {
            var parentPath = Directory.GetParent(new DirectoryInfo(path).FullName);
            if (parentPath == null)
                return String.Empty;

            return parentPath.FullName;
        }
    }
}
