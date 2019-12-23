using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using QuartzWorkerService.Jobs;

namespace QuartzWorkerService.Common
{
    public class QuartzHostedService:IHostedService
    {
        private readonly ILogger<QuartzHostedService> _logger;
        private readonly IJobFactory _jobFactory;
        private readonly IConfiguration _configuration;
        private IScheduler _scheduler;

        public QuartzHostedService(ILogger<QuartzHostedService> logger, 
            IJobFactory jobFactory, 
            IConfiguration configuration)
        {
            _logger = logger;
            _jobFactory = jobFactory;
            _configuration = configuration;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            var factory = new StdSchedulerFactory(props);
            _scheduler = await factory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;
            await _scheduler.Start(cancellationToken);

            var jobCrons = _configuration.GetSection("Jobs").Get<JobCron[]>();
            var i = 0;
            foreach (var jobCron in jobCrons)
            {
                var type = Type.GetType(jobCron.Name, false, true);
                var job = JobBuilder.Create(type)
                    .WithIdentity($"job{i}", $"group{i}")
                    .Build();
                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"trigger{i}", $"group{i}")
                    .StartNow()
                    .WithCronSchedule(jobCron.Cron)
                    .Build();
                await _scheduler.ScheduleJob(job, trigger, cancellationToken);
                i++;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
        }
    }
}