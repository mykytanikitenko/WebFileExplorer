using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace WebFileExplorer.Extensions
{
    internal class FileEnumerator : IEnumerable<FileInfo>
    {
        private readonly string path;

        public FileEnumerator(string path)
        {
            this.path = path;
        }

        public IEnumerator<FileInfo> GetEnumerator()
        {
            var dirnames = new Queue<string>();
            dirnames.Enqueue(path);
            do
            {
                var dirname = dirnames.Dequeue();
                IEnumerable<string> files = null;
                try
                {
                    files = Directory.EnumerateFiles(dirname);
                }
                catch
                {
                }

                if(files != null)
                {
                    foreach (var fname in files)
                        yield return new FileInfo(fname);
                }

                IEnumerable<string> directories = null;
                try
                {
                    directories = Directory.EnumerateDirectories(dirname);
                }
                catch
                {
                }

                if (directories != null)
                {
                    foreach (var subdir in directories)
                        dirnames.Enqueue(subdir);
                }
            }
            while (dirnames.Count > 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
