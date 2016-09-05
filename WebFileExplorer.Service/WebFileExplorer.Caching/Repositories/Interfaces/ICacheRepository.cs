using System;

namespace WebFileExplorer.Caching.Repositories.Interfaces
{
    public interface ICacheRepository<in TItem, TValue> where TItem : IEquatable<TItem> where TValue : class
    {
        TValue TryGet(TItem item);
        void Add(TItem item, TValue value);
    }
}
