using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace YfCloud.App.Module.Report
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args);
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")                
                .Build();
            #if(DEBUG)
            //DEBUG模式使用默认的启动路径
            webHost.UseConfiguration(config)
                .ConfigureLogging((hostintContext, logging) =>
                {
                    logging.AddFilter("System", LogLevel.Warning);
                    logging.AddFilter("Microsoft", LogLevel.Warning);
                    logging.AddLog4Net("log4net.config");
                })
                .UseStartup<Startup>();
            #else
            //Release模式使用appsettings.json中的urls
            webHost.UseKestrel()
                .UseConfiguration(config)
                .ConfigureLogging((hostintContext, logging) =>
                {
                    logging.AddFilter("System", LogLevel.Warning);
                    logging.AddFilter("Microsoft", LogLevel.Warning);
                    logging.AddLog4Net();
                })
                .UseStartup<Startup>();
            #endif
            return webHost;
        }
            
    }
}
