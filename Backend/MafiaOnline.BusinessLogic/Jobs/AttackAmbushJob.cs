using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.BusinessLogic.Utils;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MafiaAPI.Jobs
{

    [DisallowConcurrentExecution]
    public class AttackAmbushJob : IJob
    {
        private readonly ILogger<AttackAmbushJob> _logger;
        private readonly IAmbushService _agentService;
        private readonly IMovingAgentUtils _movingAgentUtils;
        public AttackAmbushJob(
            ILogger<AttackAmbushJob> logger,
            IAmbushService agentService,
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
            var datas = JsonSerializer.Deserialize<AttackAmbushRequest>(_movingAgentUtils.GetDatas(movingAgentId));
            _movingAgentUtils.RemoveMovingAgent(movingAgentId);
            await _agentService.AttackAmbush(datas);
        }
    }

    public class AttackAmbushJobRunner : IAttackAmbushJobRunner
    {
        private readonly IMovingAgentUtils _movingAgentUtils;
        public AttackAmbushJobRunner(IMovingAgentUtils movingAgentUtils)
        {
            _movingAgentUtils = movingAgentUtils;
        }

        public async Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId)
        {
            IScheduler scheduler = await factory.GetScheduler();

            var key = $"attackAmbushJob{movingAgentId}";

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
            return JobBuilder.Create<AttackAmbushJob>()
                .WithIdentity($"attackAmbushJob{movingAgentId}", "group1")
                .UsingJobData("movingAgentId", movingAgentId.ToString())
                .Build();
        }

        private ITrigger PrepareTrigger(long movingAgentId, DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"attackAmbushTrigger{movingAgentId}", "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

    public interface IAttackAmbushJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId);
    }


}