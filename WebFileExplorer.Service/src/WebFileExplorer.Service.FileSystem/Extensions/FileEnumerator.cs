using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebFileExplorer.Service.FileSystem.Extensions
{
    /// <summary>
    /// Recursively lazy enumerates all files and folders (because IEnumerable) and ignores exceptions
    /// </summary>
    internal class FileEnumerator : IEnumerable<FileInfo>
    {
        private readonly string path;

        public FileEnumerator(string path)
        {
            this.path = path;
        }

        public IEnumerator<FileInfo> GetEnumerator()
        {
            var directoryNames = new Queue<string>();
            directoryNames.Enqueue(path);
            do
            {
                var directoryName = directoryNames.Dequeue();
                IEnumerable<string> files = null;
                try
                {
                    files = Directory.EnumerateFiles(directoryName);
                }
                catch
                {
                }

                if(files != null)
                {
                    FileInfo file = null;
                    foreach (var fileName in files)
                    {
                        try
                        {
                            file = new FileInfo(fileName);
                        }
                        catch
                        {
                        }

                        if (file != null)
                            yield return file;
                    }
                }

                IEnumerable<string> directories = null;
                try
                {
                    directories = Directory.EnumerateDirectories(directoryName);
                }
                catch
                {
                }

                if (directories != null)
                {
                    foreach (var subdir in directories)
                        directoryNames.Enqueue(subdir);
                }
            }
            while (directoryNames.Any());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
