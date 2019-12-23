using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace QuartzWorkerService.Jobs
{
    public class TestJob:IJob
    {
        private readonly ILogger<TestJob> _logger;

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("xxoo");
            _logger.LogInformation($"test job : { DateTime.Now.ToUniversalTime() }");
            return Task.CompletedTask;
        }
    }
}