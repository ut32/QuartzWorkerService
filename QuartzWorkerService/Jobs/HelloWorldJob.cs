using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace QuartzWorkerService.Jobs
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HelloWorldJob : IJob
    {
        private readonly ILogger<TestJob> _logger;

        public HelloWorldJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("hahahaha");
            _logger.LogInformation($"hello world job : { DateTime.Now.ToUniversalTime() }");
            return Task.CompletedTask;
        }
    }
}