using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using WebFileExplorer.Service.FileSystem.Extensions;
using WebFileExplorer.Service.FileSystem.Services.Interfaces;
using WebFileExplorer.ServiceDomain.Domain;

namespace WebFileExplorer.Service.FileSystem.Services
{
    internal class FileExplorerService : IFileExplorerService
    {
        public CountFilesResult CountFilesInDirectory(string path, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(path))
                return null;

            var countFilesResult = new CountFilesResult();

            new DirectoryInfo(path)
                .ForceEnumerateFiles()
                .AsParallel()
                .TakeWhile(_ => !cancellationToken.IsCancellationRequested) // WithCancellation throws exception, so this variant is cheaper for runtime
                .ForAll(file =>
                {
                    var fileSizeMb = file.Length / 1024.0 / 1024;
                    if (fileSizeMb <= 10)
                    {
                        countFilesResult.FilesLessTen++;
                        return;
                    }

                    if (fileSizeMb >= 100)
                    {
                        countFilesResult.FilesGreaterHundred++;
                        return;
                    }

                    if (fileSizeMb > 10 && fileSizeMb <= 50)
                        countFilesResult.FilesLessFiftyAndGreaterTen++;
                });

            return countFilesResult;
        }

        public IEnumerable<FileSystemItem> GetItemsInPath(string path, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(path))
                return Enumerable.Empty<FileSystemItem>();

            var targetDirectoy = new DirectoryInfo(path);

            var files = targetDirectoy
                .SafeEnumerateFiles("*.*", SearchOption.TopDirectoryOnly)
                .AsParallel()
                .TakeWhile(_ => !cancellationToken.IsCancellationRequested)
                .Select(item => new FileSystemItem
                {
                    Location = item.DirectoryName,
                    Name = item.Name,
                    Type = FileSystemItemType.File
                })
                .OrderBy(drive => drive.Name);

            return targetDirectoy
                .SafeEnumerateDirectories("*.*", SearchOption.TopDirectoryOnly)
                .AsParallel()
                .TakeWhile(_ => !cancellationToken.IsCancellationRequested)
                .Select(item => new FileSystemItem
                {
                    Location = GetParentLocation(item),
                    Name = item.Name,
                    Type = FileSystemItemType.Directory
                })
                .OrderBy(drive => drive.Name)
                .Concat(files);
        }

        public IEnumerable<FileSystemItem> GetDrives(CancellationToken cancellationToken)
        {
            return DriveInfo
                .GetDrives()
                .AsParallel()
                .TakeWhile(_ => !cancellationToken.IsCancellationRequested)
                .Where(drive => drive.IsReady)
                .Select(drive => new FileSystemItem
                {
                    Name = drive.Name,
                    Location = drive.RootDirectory.Name,
                    Type = FileSystemItemType.Drive
                })
                .OrderBy(drive => drive.Name);
        }

        private static string GetParentLocation(DirectoryInfo item)
        {
            var parentPath = Directory.GetParent(item.FullName);
            if (parentPath == null)
                return String.Empty;

            return parentPath.FullName;
        }
    }
}
