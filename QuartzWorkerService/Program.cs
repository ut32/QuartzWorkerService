using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz.Spi;
using QuartzWorkerService.Common;
using QuartzWorkerService.Jobs;

namespace QuartzWorkerService
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }   

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) =>
                {  
                    //builder.AddFilter("System", LogLevel.Warning); //过滤掉系统默认的一些日志
                    //builder.AddFilter("Microsoft", LogLevel.Warning);//过滤掉系统默认的一些日志
  
                    builder.AddLog4Net();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    //add job factory
                    services.AddSingleton<IJobFactory, JobFactory>();
                    
                    //add job
                    services.AddSingleton<TestJob>();
                    services.AddSingleton<HelloWorldJob>();

                    services.AddHostedService<QuartzHostedService>();
                });
    }
}