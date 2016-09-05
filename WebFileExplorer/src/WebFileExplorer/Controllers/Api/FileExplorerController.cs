using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebFileExplorer.ApiDomain.FileExplorer.Responses;
using WebFileExplorer.ServiceDomain.Api.Responses;
using WebFileExplorer.Services.Interfaces;

namespace WebFileExplorer.Controllers.Api
{
    [Route("api/[controller]")]
    public class FileExplorerController : Controller
    {
        private readonly IFileSystemClientService fileSystemClientService;

        public FileExplorerController(IFileSystemClientService fileSystemClientService)
        {
            this.fileSystemClientService = fileSystemClientService;
        }

        [HttpGet]
        [Route("[action]/{appToken}/cached/{cached}")]
        public async Task<IEnumerable<FileSystemItemResponse>> Drives(string appToken, bool cached)
        {
            var token = new Guid(appToken);
            return await fileSystemClientService.RequestDrivesAsync(token, cached);
        }

        [HttpGet]
        [Route("[action]/{appToken}/cached/{cached}/path/{pathBase64}")]
        public async Task<IEnumerable<FileSystemItemResponse>> Entries(string appToken, bool cached, string pathBase64)
        {
            var path = DecodeBase64(pathBase64);
            if (String.IsNullOrWhiteSpace(path))
                return null;

            var token = new Guid(appToken);
            return await fileSystemClientService.RequestEntriesAsync(path, token, cached);
        }

        [HttpGet]
        [Route("[action]/{appToken}/cached/{cached}/path/{pathBase64}")]
        public async Task<IEnumerable<FileSystemItemResponse>> ParentEntries(string appToken, bool cached, string pathBase64)
        {
            var path = DecodeBase64(pathBase64);
            if (String.IsNullOrWhiteSpace(path))
                return null;

            var token = new Guid(appToken);
            return await fileSystemClientService.RequstEntiesInParentAsync(path, token, cached);
        }

        [HttpGet]
        [Route("[action]/{appToken}/cached/{cached}/path/{pathBase64}")]
        public async Task<CountFilesResponse> CountFiles(string appToken, bool cached, string pathBase64)
        {
            var path = DecodeBase64(pathBase64);
            if (String.IsNullOrWhiteSpace(path))
                return null;

            var token = new Guid(appToken);
            return await fileSystemClientService.RequestCountFilesAsync(path, token, cached);
        }

        [HttpGet]
        [Route("[action]/{appToken}/cached/{cached}/path/{pathBase64}")]
        public async Task<CountFilesResponse> ParentCountFiles(string appToken, bool cached, string pathBase64)
        {
            var path = DecodeBase64(pathBase64);
            if (String.IsNullOrWhiteSpace(path))
                return null;

            var token = new Guid(appToken);
            return await fileSystemClientService.RequestParentCountFilesAsync(path, token, cached);
        }

        private static string DecodeBase64(string encodedString)
        {
            var decodedBytes = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}
