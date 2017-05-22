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
        private static ConnectionMultiplexer connectionMultiplexer;
        private static IDatabase database;

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
            _defaultExpirationTime = new TimeSpan(168, 0, 0);

            //use locally redis installation
            //var connectionString = string.Format("{0}:{1}", "127.0.0.1", 6379); //localhost
            var connectionString = string.Format("{0}:{1}", "10.51.4.94", 6379); //jamir
            //var connectionString = string.Format("{0}:{1}", "10.51.5.23", 6379); //romulo

            connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            database = connectionMultiplexer.GetDatabase();
        }

        private const string _generateKeyStringFormat = "{0}.Id = {1}";

        public bool Exists<T>(long entityId) where T : class
        {
            var key = this.GenerateKey(typeof(T), entityId);

            return database.KeyExists(key);
        }

        public T Get<T>(long entityId) where T : class
        {
            string key = this.GenerateKey(typeof(T), entityId);

            var obj = database.StringGet(key);

            return JsonConvert.DeserializeObject<T>(obj);
        }

        public void Add<T>(T entity, long entityId, TimeSpan? expirationTime = null) where T : class
        {
            string key = this.GenerateKey(typeof(T), entityId);

            if (!this.Exists<T>(entityId))
            {
                var expiration = this.GetExpirationTime(expirationTime);

                var serializedObject = JsonConvert.SerializeObject(entity);
                database.StringSet(key, serializedObject, expiration);
            }
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

        public void Remove<T>(long entityId) where T : class
        {
            if (this.Exists<T>(entityId))
            {
                string key = this.GenerateKey(typeof(T), entityId);
                database.KeyDelete(key);
            }
        }



        //public void ReadData()
        //{
        //var cache = RedisConnectorHelper.Connection.GetDatabase();
        //var devicesCount = 10000;
        //for (int i = 0; i < devicesCount; i++)
        //{
        //    var value = cache.StringGet($"Device_Status:{i}");
        //    Console.WriteLine($"Valor={value}");
        //}
        //}

        //public void SaveBigData()
        //{
        //var devicesCount = 10000;
        //var rnd = new Random();
        //var cache = RedisConnectorHelper.Connection.GetDatabase();

        //for (int i = 1; i < devicesCount; i++)
        //{
        //    var value = rnd.Next(0, 10000);
        //    cache.StringSet($"Device_Status:{i}", value);
        //}
        //}
    }
}
