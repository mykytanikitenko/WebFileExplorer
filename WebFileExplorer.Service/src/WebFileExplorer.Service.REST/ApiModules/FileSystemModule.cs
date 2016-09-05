using System;
using Nancy;
using Nancy.ModelBinding;
using WebFileExplorer.Service.REST.Services.Interfaces;
using WebFileExplorer.ServiceDomain.Api.Requests;

namespace WebFileExplorer.Service.REST.ApiModules
{
    public class FileSystemModule : NancyModule
    {
        private readonly IRestFileExplorerService restFileExplorerService;

        public FileSystemModule(IRestFileExplorerService restFileExplorerService)
        {
            this.restFileExplorerService = restFileExplorerService;
            BindRoutes();
        }

        private void BindRoutes()
        {
            Post["FileSystem/Entries"] = Entries;
            Post["FileSystem/Drives"] = Drives;
            Post["FileSystem/Count"] = Count;
        }

        private dynamic Entries(dynamic parameters)
        {
            var request = this.Bind<FileSystemRequest>();
            if(String.IsNullOrWhiteSpace(request.Path))
                return Response.AsJson(restFileExplorerService.GetDrives(request));

            return Response.AsJson(restFileExplorerService.GetFileSystemEntries(request));
        }

        private dynamic Drives(dynamic parameters)
        {
            var request = this.Bind<FileSystemRequest>();

            return Response.AsJson(restFileExplorerService.GetDrives(request));
        }

        private dynamic Count(dynamic parametrs)
        {
            var request = this.Bind<FileSystemRequest>();

            return Response.AsJson(restFileExplorerService.CountFiles(request));
        }
    }
}
