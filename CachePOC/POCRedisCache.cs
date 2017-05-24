using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace CachePOC
{
    public class POCRedisCache
    {
        private static object syncRoot = new object();
        private static volatile POCRedisCache _instance;
        private readonly TimeSpan _defaultExpirationTime;
        private static IDatabase databaseMaster;
        private static IDatabase databaseSlave;
        private const string _generateKeyStringFormat = "{0}.Id = {1}";

        public static POCRedisCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new POCRedisCache();
                    }
                }

                return _instance;
            }
        }

        public POCRedisCache()
        {
            _defaultExpirationTime = new TimeSpan(1, 0, 0);

            //use locally redis installation
            var connectionStringMaster = string.Format("{0}:{1}", "127.0.0.1", 6379); //localhost master

            var connectionMultiplexerMaster = ConnectionMultiplexer.Connect(connectionStringMaster);
            databaseMaster = connectionMultiplexerMaster.GetDatabase();

            var connectionStringSlave = string.Format("{0}:{1}", "127.0.0.1", 6380); //localhost slave

            var connectionMultiplexerSlave = ConnectionMultiplexer.Connect(connectionStringSlave);
            databaseSlave = connectionMultiplexerSlave.GetDatabase();
        }

        public bool Exists<T>(long entityId) where T : class
        {
            var key = this.GenerateKey(typeof(T), entityId);

            return databaseMaster.KeyExists(key);
        }

        public void Add<T>(T entity, long entityId, TimeSpan? expirationTime = null) where T : class
        {
            string key = this.GenerateKey(typeof(T), entityId);

            //if (!this.Exists<T>(entityId))
            {
                var expiration = this.GetExpirationTime(expirationTime);

                var serializedObject = JsonConvert.SerializeObject(entity);
                databaseMaster.StringSet(key, serializedObject, expiration);
            }
        }

        public T Get<T>(long entityId) where T : class
        {
            string key = this.GenerateKey(typeof(T), entityId);

            var obj = databaseSlave.StringGet(key);

            return JsonConvert.DeserializeObject<T>(obj);
        }

        //put
        //public void Update<T>(long entityId, TimeSpan? expirationTime = null) where T : class
        //{
        //    databaseMaster.StringSet()
        //}

        public void Remove<T>(long entityId) where T : class
        {
            if (this.Exists<T>(entityId))
            {
                string key = this.GenerateKey(typeof(T), entityId);
                databaseMaster.KeyDelete(key);
            }
        }

        #region private methods 

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

        #endregion
    }
}
