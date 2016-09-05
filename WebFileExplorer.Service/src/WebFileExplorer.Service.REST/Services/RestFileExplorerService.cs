using System;
using System.Collections.Generic;
using System.Linq;
using WebFileExplorer.Caching.Repositories.Interfaces;
using WebFileExplorer.Service.Core.Concurrency;
using WebFileExplorer.Service.Core.Concurrency.Interfaces;
using WebFileExplorer.Service.FileSystem.Services.Interfaces;
using WebFileExplorer.Service.REST.Configuration;
using WebFileExplorer.Service.REST.Services.Interfaces;
using WebFileExplorer.ServiceDomain.Api.Requests;
using WebFileExplorer.ServiceDomain.Api.Responses;
using WebFileExplorer.ServiceDomain.Domain;

namespace WebFileExplorer.Service.REST.Services
{
    internal class RestFileExplorerService : IRestFileExplorerService
    {
        private readonly IFileExplorerService fileExplorerService;
        private readonly ICacheRepository<string, CountFilesResponse> countFilesCacheRepository;
        private readonly ICacheRepository<string, IEnumerable<FileSystemItem>> fileSystemItemsCacheRepository;
        private readonly IRequestPool requestPool;
        private readonly IRequestConfiguration requestConfiguration;

        // To identify operations
        private Guid CountOperationsGroup => new Guid("d51ec59d-b185-4d07-b4d4-5926498502bf");
        private Guid EploreFilesOperationsGroup => new Guid("c9b8ddb1-b781-4952-a75c-8fa889cabd52");

        public RestFileExplorerService(
            IFileExplorerService fileExplorerService,
            ICacheRepository<string, CountFilesResponse> countFilesCacheRepository,
            ICacheRepository<string, IEnumerable<FileSystemItem>> fileSystemItemsCacheRepository,
            IRequestPool requestPool,
            IRequestConfiguration requestConfiguration)
        {
            this.fileExplorerService = fileExplorerService;
            this.countFilesCacheRepository = countFilesCacheRepository;
            this.fileSystemItemsCacheRepository = fileSystemItemsCacheRepository;
            this.requestPool = requestPool;
            this.requestConfiguration = requestConfiguration;
        }

        public CountFilesResponse CountFiles(FileSystemRequest request)
        {
            if (String.IsNullOrWhiteSpace(request.Path))
                return null;

            requestPool.CancellAllFromSourceInGroup(request.SourceToken, CountOperationsGroup);

            if (request.Cached)
            {
                var cached = countFilesCacheRepository.TryGet(request.Path);
                if (cached != null)
                    return cached;
            }

            return DoCount(request);
        }

        public IEnumerable<FileSystemItem> GetFileSystemEntries(FileSystemRequest request)
        {
            requestPool.CancellAllFromSourceInGroup(request.SourceToken, EploreFilesOperationsGroup);

            if (request.Cached)
            {
                var cached = fileSystemItemsCacheRepository.TryGet(request.Path);
                if (cached != null)
                    return cached;
            }

            return DoGetEntries(request);
        }

        public IEnumerable<FileSystemItem> GetDrives(FileSystemRequest request)
        {
            requestPool.CancellAllFromSourceInGroup(request.SourceToken, EploreFilesOperationsGroup);

            if (request.Cached)
            {
                var cached = fileSystemItemsCacheRepository.TryGet("DrivesListCache");
                if (cached != null)
                    return cached;
            }

            return DoGetDrives(request);
        }

        private CountFilesResponse DoCount(FileSystemRequest request)
        {
            var cancellableOperation = new CancellableOperation<string, CountFilesResult>(
                fileExplorerService.CountFilesInDirectory, request.Path, request.SourceToken, CountOperationsGroup);

            requestPool.Add(cancellableOperation);

            cancellableOperation.CancelAfter(requestConfiguration.ExecutionTimeout);
            cancellableOperation.Operation.Wait();

            var result = cancellableOperation.Operation.Result;
            var response = CreateCountFilesResponse(result, cancellableOperation.Cancelled);
            countFilesCacheRepository.Add(request.Path, response);

            return response;
        }

        private IEnumerable<FileSystemItem> DoGetEntries(FileSystemRequest request)
        {
            var cancellableOperation = new CancellableOperation<string, IEnumerable<FileSystemItem>>(
                fileExplorerService.GetItemsInPath, request.Path, request.SourceToken, EploreFilesOperationsGroup);

            cancellableOperation.CancelAfter(requestConfiguration.ExecutionTimeout);
            cancellableOperation.Operation.Wait();

            var result = cancellableOperation.Operation.Result;
            if (result.Any())
                fileSystemItemsCacheRepository.Add(request.Path, result.ToArray());

            return result;
        }

        private IEnumerable<FileSystemItem> DoGetDrives(FileSystemRequest request)
        {
            var cancellableOperation = new CancellableOperation<string, IEnumerable<FileSystemItem>>(
                fileExplorerService.GetDrives, request.SourceToken, EploreFilesOperationsGroup);

            cancellableOperation.CancelAfter(requestConfiguration.ExecutionTimeout);
            cancellableOperation.Operation.Wait();

            var result = cancellableOperation.Operation.Result;
            if (result.Any())
                fileSystemItemsCacheRepository.Add("DrivesListCache", result.ToArray());

            return result;
        }

        private static CountFilesResponse CreateCountFilesResponse(CountFilesResult countFilesResult, bool cancelled)
        {
            return new CountFilesResponse
            {
                FilesGreaterHundred = countFilesResult.FilesGreaterHundred,
                FilesLessFiftyAndGreaterTen = countFilesResult.FilesLessFiftyAndGreaterTen,
                FilesLessTen = countFilesResult.FilesLessTen,
                Timeouted = cancelled
            };
        }
    }
}
 