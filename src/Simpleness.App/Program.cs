using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Simpleness.DataEntityFramework.SeedData;

namespace Simpleness.App
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Info("程序启动");
                var host = CreateWebHostBuilder(args).Build();

                //初始化数据数据
                using (var scpoe = host.Services.CreateScope())
                {
                    var services = scpoe.ServiceProvider;
                    AppSeedData.InitAsync(services).Wait();
                }
                host.Run();

            }
            catch (Exception ex)
            {
                logger.Error($"启动程序出错: { ex.Message}-{ex.InnerException?.Message}");
                throw;
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseNLog();
    }
#pragma warning restore CS1591
}
