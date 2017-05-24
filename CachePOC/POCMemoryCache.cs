using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace CachePOC
{
    public sealed class POCMemoryCache
    {
        private readonly TimeSpan _defaultExpirationTime;
        private static volatile POCMemoryCache _instance;
        private static object syncRoot = new object();
        private bool _cacheEnable = true;
        private const string _generateKeyStringFormat = "{0}.Id = {1}";

        private IEnumerable<object> all
        {
            get { return MemoryCache.Default.Select(p => p.Value); }
        }

        public bool CacheEnable
        {
            get { return _cacheEnable; }
            set { _cacheEnable = value; }
        }

        public static POCMemoryCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new POCMemoryCache();
                    }
                }

                return _instance;
            }
        }

        private POCMemoryCache()
        {
            _defaultExpirationTime = new TimeSpan(168, 0, 0);
        }

        public void Add<T>(T entity, long entityId, TimeSpan? expirationTime = null) where T : class
        {
            if (!_cacheEnable)
                return;

            CacheItemPolicy cacheItemPolicy = this.GetCacheItemPolicy(expirationTime);
            string key = this.GenerateKey(typeof(T), entityId);

            if (!this.Exists<T>(entityId))
            {
                MemoryCache.Default.Add(key, entity, cacheItemPolicy);
            }
        }

        public void Add<T>(IEnumerable<T> list, Func<T, long> idSelector, TimeSpan? expirationTime = null) where T : class
        {
            if (!_cacheEnable)
                return;

            foreach (T item in list)
            {
                this.Add(item, idSelector(item), expirationTime);
            }
        }

        public void Clear<T>() where T : class
        {
            string keyToRemove = String.Format("{0}.Id = ", typeof(T));

            List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).Where(a => a.StartsWith(keyToRemove)).ToList();

            foreach (string cacheKey in cacheKeys)
            {
                MemoryCache.Default.Remove(cacheKey);
            }
        }

        public IEnumerable<T> Get<T>() where T : class
        {
            return this.all.OfType<T>();
        }

        public T Get<T>(long entityId) where T : class
        {
            string key = this.GenerateKey(typeof(T), entityId);
            var obj = (T)MemoryCache.Default.Get(key);

            //GetChildren(obj);

            return obj;
        }

        public IEnumerable<T> Get<T>(Func<T, bool> predicate) where T : class
        {
            return this.all.OfType<T>().Where(predicate);
        }

        public bool Exists<T>(long entityId) where T : class
        {
            var key = this.GenerateKey(typeof(T), entityId);
            return MemoryCache.Default.Contains(key);
        }

        public void Remove<T>(long entityId) where T : class
        {
            if (this.Exists<T>(entityId))
            {
                string key = this.GenerateKey(typeof(T), entityId);
                MemoryCache.Default.Remove(key);
            }
        }

        public void RemoveAll<T>(Func<T, bool> predicate, Func<T, long> idSelector) where T : class
        {
            var items = this.Get(predicate);
            foreach (var item in items)
            {
                this.Remove<T>(idSelector(item));
            }
        }

        public bool Any<T>() where T : class
        {
            return this.Get<T>().Any();
        }

        private CacheItemPolicy GetCacheItemPolicy(TimeSpan? expirationTime = null)
        {
            if (expirationTime.HasValue)
                return new CacheItemPolicy { SlidingExpiration = expirationTime.Value };
            else
                return new CacheItemPolicy { SlidingExpiration = _defaultExpirationTime };
        }

        private string GenerateKey(Type type, long entityId)
        {
            return string.Format(_generateKeyStringFormat, type.FullName, entityId);
        }

        //private void GetChildren<T>(T obj)
        //{
        //    Type type = obj.GetType();

        //    if (type.IsClass)
        //    {
        //        foreach (PropertyInfo curPropInfo in type.GetProperties().Where(a => a.PropertyType.IsClass &&
        //                                                                             !a.PropertyType.IsEnum &&
        //                                                                             !a.PropertyType.IsArray &&
        //                                                                             !a.PropertyType.IsValueType &&
        //                                                                             !IsPrimitive(a.PropertyType)))
        //        {

        //        }
        //    }
        //}

        //private bool IsPrimitive(Type type)
        //{
        //    return type.IsPrimitive ||
        //        type.IsEnum ||
        //        type == typeof(string) ||
        //        type == typeof(byte) ||
        //        type == typeof(int) ||
        //        type == typeof(long) ||
        //        type == typeof(decimal) ||
        //        type == typeof(double) ||
        //        type == typeof(DateTime) ||
        //        type == typeof(TimeSpan);
        //}
    }
}
