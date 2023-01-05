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
    public class ReturnWithLootJob : IJob
    {
        private readonly ILogger<ReturnWithLootJob> _logger;
        private readonly IAgentService _agentService;
        private readonly IMovingAgentUtils _movingAgentUtils;
        public ReturnWithLootJob(
            ILogger<ReturnWithLootJob> logger,
            IAgentService agentService,
            IMovingAgentUtils movingAgentUtils
            )
        {
            _logger = logger;
            _agentService = agentService;
            _movingAgentUtils = movingAgentUtils;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            long movingAgentId = long.Parse(dataMap.GetString("movingAgentId"));
            await _agentService.MakeStepMovingWithLoot(movingAgentId);
        }
    }

    public class ReturnWithLootJobRunner : IReturnWithLootJobRunner
    {
        private readonly IMovingAgentUtils _movingAgentUtils;
        public ReturnWithLootJobRunner(IMovingAgentUtils movingAgentUtils)
        {
            _movingAgentUtils = movingAgentUtils;   
        }

        public async Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId)
        {
            IScheduler scheduler = await factory.GetScheduler();

            var key = $"returnWithLootJob{movingAgentId}";

            await _movingAgentUtils.SetJobKey(movingAgentId, key);

            JobKey jobKey = new(key, "group1");
            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.DeleteJob(jobKey);
            }

            IJobDetail newJob = PrepareJobDetail(movingAgentId);
            ITrigger trigger = PrepareTrigger(movingAgentId, finishTime);

            await scheduler.ScheduleJob(newJob, trigger);
            await scheduler.Start();
        }

        private IJobDetail PrepareJobDetail(long movingAgentId)
        {
            return JobBuilder.Create<ReturnWithLootJob>()
                .WithIdentity($"returnWithLootJob{movingAgentId}", "group1")
                .UsingJobData("movingAgentId", movingAgentId.ToString())
                .Build();
        }

        private ITrigger PrepareTrigger(long movingAgentId, DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"returnWithLootTrigger{movingAgentId}", "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

    public interface IReturnWithLootJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId);
    }



}