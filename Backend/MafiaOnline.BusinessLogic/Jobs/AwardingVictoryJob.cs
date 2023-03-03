using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MafiaAPI.Jobs
{

    [DisallowConcurrentExecution]
    public class AwardingVictoryJob : IJob
    {
        private readonly ILogger<AwardingVictoryJob> _logger;
        private readonly IBossService _bossService;
        public AwardingVictoryJob(
            ILogger<AwardingVictoryJob> logger,
            IBossService bossService
            )
        {
            _logger = logger;
            _bossService = bossService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _bossService.AwardingVictory();
        }
    }

    public class AwardingVictoryJobRunner : IAwardingVictoryJobRunner
    {
        public AwardingVictoryJobRunner()
        {
        }

        public async Task Start(ISchedulerFactory factory)
        {
            IScheduler scheduler = await factory.GetScheduler();

            var key = $"awardingVictory";

            JobKey jobKey = new(key, "group1");
            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.DeleteJob(jobKey);
            }

            IJobDetail newJob = PrepareJobDetail();
            ITrigger trigger = PrepareTrigger();

            await scheduler.ScheduleJob(newJob, trigger);
            await scheduler.Start();
        }

        private IJobDetail PrepareJobDetail()
        {
            return JobBuilder.Create<AwardingVictoryJob>()
                .WithIdentity($"awardingVictoryJob", "group1")
                .Build();
        }

        private ITrigger PrepareTrigger()
        {
            return TriggerBuilder.Create()
                .WithIdentity($"awardingVictoryTrigger", "group1")
                .WithCronSchedule("0 0 0,18 ? * SUN *")
                .Build();
        }
    }

    public interface IAwardingVictoryJobRunner
    {
        Task Start(ISchedulerFactory factory);
    }



}