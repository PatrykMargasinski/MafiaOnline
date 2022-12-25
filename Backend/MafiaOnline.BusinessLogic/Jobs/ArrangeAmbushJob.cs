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
    public class ArrangeAmbushJob : IJob
    {
        private readonly ILogger<ArrangeAmbushJob> _logger;
        private readonly IAmbushService _ambushService;
        private readonly IMovingAgentUtils _movingAgentUtils;
        public ArrangeAmbushJob(
            ILogger<ArrangeAmbushJob> logger,
            IAmbushService ambushService,
            IMovingAgentUtils movingAgentUtils
            )
        {
            _logger = logger;
            _ambushService = ambushService;
            _movingAgentUtils = movingAgentUtils;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            long movingAgentId = long.Parse(dataMap.GetString("movingAgentId"));
            var datas = JsonSerializer.Deserialize<ArrangeAmbushRequest>(_movingAgentUtils.GetDatas(movingAgentId));
            _movingAgentUtils.RemoveMovingAgent(movingAgentId);
            await _ambushService.ArrangeAmbush(datas);
        }
    }

    public class ArrangeAmbushJobRunner : IArrangeAmbushJobRunner
    {
        private readonly IMovingAgentUtils _movingAgentUtils;
        public ArrangeAmbushJobRunner(IMovingAgentUtils movingAgentUtils)
        {
            _movingAgentUtils = movingAgentUtils;   
        }

        public async Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId)
        {
            IScheduler scheduler = await factory.GetScheduler();

            var key = $"arrangeAmbushJob{movingAgentId}";

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
            return JobBuilder.Create<ArrangeAmbushJob>()
                .WithIdentity($"arrangeAmbushJob{movingAgentId}", "group1")
                .UsingJobData("movingAgentId", movingAgentId.ToString())
                .Build();
        }

        private ITrigger PrepareTrigger(long movingAgentId, DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"arrangeAmbushTrigger{movingAgentId}", "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

    public interface IArrangeAmbushJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId);
    }



}