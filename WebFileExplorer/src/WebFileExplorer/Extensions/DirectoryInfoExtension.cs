using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebFileExplorer.Extensions
{
    public static class DirectoryInfoExtension
    {
        /// <summary>
        /// Wraps method EnumerateFiles with ToSafeEnumerable.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> SafeEnumerateFiles(this DirectoryInfo source, string pattern, SearchOption searchOption)
        {
            return Directory
                .EnumerateFiles(source.FullName, pattern, searchOption)
                .ToSafeEnumerable()
                .Select(file => new FileInfo(file));
        }

        /// <summary>
        /// Wraps method EnumerateDirectories with ToSafeEnumerable. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static IEnumerable<DirectoryInfo> SafeEnumerateDirectories(this DirectoryInfo source, string pattern, SearchOption searchOption)
        {
            return Directory
                .EnumerateDirectories(source.FullName, pattern, searchOption)
                .ToSafeEnumerable()
                .Select(dir => new DirectoryInfo(dir));
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
