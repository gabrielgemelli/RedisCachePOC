using System;
using Glav.CacheAdapter;
using Glav.CacheAdapter.Core;
using Glav.CacheAdapter.Core.Diagnostics;
using Glav.CacheAdapter.Helpers;

namespace CachePOC
{
    public class POCCacheAdapter
    {
        private static object syncRoot = new object();
        private readonly TimeSpan _defaultExpirationTime;
        private const string _generateKeyStringFormat = "{0}.Id = {1}";
        private readonly ICacheProvider _cacheProvider;
        private readonly ConsoleLogger _logger;

        private static volatile POCCacheAdapter _instance;

        public static POCCacheAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new POCCacheAdapter();
                    }
                }

                return _instance;
            }
        }

        private POCCacheAdapter()
        {
            _logger = new ConsoleLogger();
            _cacheProvider = CacheConfig.Create()
                .BuildCacheProvider(_logger);

            _defaultExpirationTime = TimeSpan.FromMinutes(20);
        }

        public void SetDependency<TParent, TChild>(long parentId, long childId)
        {
            var parentKey = GenerateKey(typeof(TParent), parentId);
            var childKey = GenerateKey(typeof(TChild), childId);

            _cacheProvider.InnerDependencyManager.AssociateDependentKeysToParent(parentKey, new[] { childKey });
        }

        public bool Exists<T>(long entityId) where T : class
        {
            var key = GenerateKey(typeof(T), entityId);
            return _cacheProvider.InnerCache.Get<T>(key) != null;
        }

        public void Add<T>(T entity, long entityId, TimeSpan? expirationTime = null) where T : class
        {
            Remove<T>(entityId);

            var key = GenerateKey(typeof(T), entityId);
            var expiration = GetExpirationTime(expirationTime);
            _cacheProvider.Add(key, expiration, entity);
        }

        public T Get<T>(long entityId) where T : class
        {
            string key = GenerateKey(typeof(T), entityId);
            return _cacheProvider.InnerCache.Get<T>(key);
        }

        public void Remove<T>(long entityId) where T : class
        {
            if (Exists<T>(entityId))
            {
                var key = GenerateKey(typeof(T), entityId);
                _cacheProvider.InvalidateCacheItem(key);
            }
        }

        public void Clear()
        {
            _cacheProvider.InnerCache.ClearAll();
        }

        public void DisableLogging()
        {
            _logger.Enabled = false;
        }

        public void EnableLogging()
        {
            _logger.Enabled = true;
        }

        private string GenerateKey(Type type, long entityId)
        {
            return string.Format(_generateKeyStringFormat, type.FullName, entityId);
        }

        private TimeSpan GetExpirationTime(TimeSpan? expirationTime = null)
        {
            if (expirationTime.HasValue)
                return expirationTime.Value;
            else
                return _defaultExpirationTime;
        }

        class ConsoleLogger : ILogging
        {
            public bool Enabled { get; set; }

            public void WriteErrorMessage(string message)
            {
                if (Enabled)
                {
                    Console.WriteLine(message);
                }
            }

            public void WriteException(Exception ex)
            {
                if (Enabled)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void WriteInfoMessage(string message)
            {
                if (Enabled)
                {
                    Console.WriteLine(message);
                }
            }
        }
    }
}
