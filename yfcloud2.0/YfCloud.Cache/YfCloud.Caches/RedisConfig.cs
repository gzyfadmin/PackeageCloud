using Microsoft.Extensions.Configuration;
using System.IO;

namespace YfCloud.Caches
{
    /// <summary>
    /// Redis配置信息
    /// </summary>
    public class RedisConfigs
    {
        /// <summary>
        /// 默认构造方法
        /// </summary>
        public RedisConfigs()
        {
            SetConfig();
        }

        private void SetConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(System.Environment.CurrentDirectory, "appsettings.json"));
            var config = builder.Build();
            Host = config.GetSection("CacheHost").Value.ToString();
            Port = int.Parse(config.GetSection("CachePort").Value.ToString());
            Expired = int.Parse(config.GetSection("CacheExpired").Value.ToString());
        }

        /// <summary>
        /// 缓存服务IP地址
        /// </summary>
        public  string Host { get; private set; }
        /// <summary>
        /// 缓存服务端口号
        /// </summary>
        public  int Port { get; private set; }

        /// <summary>
        /// 缓存过期,单位秒
        /// </summary>
        public  int Expired { get; private set; }
    }
}
