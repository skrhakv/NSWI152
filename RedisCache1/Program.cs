using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;

namespace RedisCache1
{
    class Program
    {
        static void Main(string[] args)
        {
            cachingGet("Message");

            cachingSet("Message", "Hello! The cache is working from a .NET console app!");

            cachingGet("Message");


            lazyConnection.Value.Dispose();
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        private static void cachingGet(string key)
        {
            IDatabase cache = Connection.GetDatabase();

            string cacheCommand = "Get " + key;

            Console.WriteLine("\nCache command  : " + cacheCommand + " or StringGet()");
            Console.WriteLine("Cache response : " + cache.StringGet("Message").ToString());

        }

        private static void cachingSet(string key, string value)
        {
            IDatabase cache = Connection.GetDatabase();

            string cacheCommand = "Set " + key + " " + value;

            Console.WriteLine("\nCache command  : " + cacheCommand + " or StringSet()");
            Console.WriteLine("Cache response : " + cache.StringSet(key, value).ToString());

        }
    }

}
