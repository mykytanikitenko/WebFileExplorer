using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebFileExplorer.Service.FileSystem.Extensions
{
    public static class DirectoryInfoExtension
    {
        /// <summary>
        /// Wraps method EnumerateFiles with try-catch
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> SafeEnumerateFiles(this DirectoryInfo source, string pattern, SearchOption searchOption)
        {
            IEnumerable<FileInfo> files;

            try
            {
                files = Directory
                    .EnumerateFiles(source.FullName, pattern, searchOption)
                    .Select(fileName => new FileInfo(fileName));
            }
            catch
            {
                files = Enumerable.Empty<FileInfo>();
            }

            return files;
        }

        /// <summary>
        /// Wraps method EnumerateDirectories with try-catch
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static IEnumerable<DirectoryInfo> SafeEnumerateDirectories(this DirectoryInfo source, string pattern, SearchOption searchOption)
        {
            IEnumerable<DirectoryInfo> directories;

            try
            {
                directories = Directory
                    .EnumerateDirectories(source.FullName, pattern, searchOption)
                    .Select(dir => new DirectoryInfo(dir));
            }
            catch
            {
                directories = Enumerable.Empty<DirectoryInfo>();
            }

            return directories;
        }

        /// <summary>
        /// Enumeration will not stop even if file will be unaccessible
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> ForceEnumerateFiles(this DirectoryInfo source)
        {
            return new FileEnumerator(source.FullName);
        } 
    }
}
