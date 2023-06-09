using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajProje.Helper
{
    public class AzureRedisCacheService
    {
        private IConfiguration _configuration;
        private readonly RedisEndpoint _redisConfiguration;
        public IConfiguration Configuration { get; }
        public AzureRedisCacheService(IConfiguration configuration)
        {
            Configuration = configuration;
            _redisConfiguration = new RedisEndpoint() {
                Host = "",
                Password = "",
                Port =0
            };
        }
        public void Set<T>(string key, T value, TimeSpan time) where T : class
        {
            using (IRedisClient client = new RedisClient(_redisConfiguration))
            {
                client.Set(key, value, time);
            }
        }
        public T Get<T>(string key) where T : class
        {
            using (IRedisClient client = new RedisClient(_redisConfiguration))
            {
                return client.Get<T>(key);
            }
        }

    }
}
