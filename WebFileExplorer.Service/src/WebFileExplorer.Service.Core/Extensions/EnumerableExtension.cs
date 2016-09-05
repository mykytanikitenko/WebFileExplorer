using System.Collections.Generic;

namespace WebFileExplorer.Service.Core.Extensions
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// Wraps Enumerator.MoveNext() with try-catch block. If MoveNext() will throw exception,
        /// iteration will be broken with catched exception and it will be silent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToSafeEnumerable<T>(this IEnumerable<T> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                bool? hasCurrent;
                do
                {
                    try
                    {
                        hasCurrent = enumerator.MoveNext();
                    }
                    catch
                    {
                        hasCurrent = null;
                    }

                    if (hasCurrent ?? false)
                        yield return enumerator.Current;

                } while (hasCurrent ?? true);
            }
        }
    }
}
