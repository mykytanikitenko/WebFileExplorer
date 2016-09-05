using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WebFileExplorer.Caching.Repositories.Interfaces;

namespace WebFileExplorer.Caching.Repositories
{
    /// <summary>
    /// Very simple implementation of in-memory caching using Dictionary
    /// 
    /// This class have to be registeed for particular scope, because it have to be singleton
    /// You have care about lifecycle of instance of this class, because it contains cache in memory
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CacheRepository<TItem, TValue> : ICacheRepository<TItem, TValue> where TItem : IEquatable<TItem> where TValue : class
    {
        private readonly ConcurrentDictionary<TItem, TValue> cache;

        public CacheRepository()
        {
            cache = new ConcurrentDictionary<TItem, TValue>();
        }
         
        public TValue TryGet(TItem item)
        {
            if (item == null || !cache.ContainsKey(item))
                return null;

            return cache[item];
        }

        public void Add(TItem item, TValue value)
        {
            if (item == null || value == null)
                return;

            TValue tmp;
            if (cache.ContainsKey(item))
                cache.TryRemove(item, out tmp);

            cache.TryAdd(item, value);
        }
    }
}
