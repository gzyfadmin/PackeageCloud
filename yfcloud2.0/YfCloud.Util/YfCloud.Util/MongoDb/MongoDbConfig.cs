using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace YfCloud.Utilities.MongoDb
{
    /// <summary>
    /// Mongodb配置文件
    /// </summary>
    public class MongoDbConfig
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MongoDbConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.json"));
            var config = builder.Build();
            Host = config.GetSection("MongoDbHost").Value.ToString();
            Port = int.Parse(config.GetSection("MongoDbPort").Value.ToString());
        }

        /// <summary>
        /// IP地址
        /// </summary>

        public string Host { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }
    }
}
