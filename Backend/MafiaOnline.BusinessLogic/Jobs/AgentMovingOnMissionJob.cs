using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities.Mission;
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
    public class AgentMovingOnMissionJob : IJob
    {
        private readonly ILogger<AgentMovingOnMissionJob> _logger;
        private readonly IMissionService _missionService;
        private readonly IMovingAgentUtils _movingAgentUtils;
        public AgentMovingOnMissionJob(
            ILogger<AgentMovingOnMissionJob> logger,
            IMissionService missionService,
            IMovingAgentUtils movingAgentUtils
            )
        {
            _logger = logger;
            _missionService = missionService;
            _movingAgentUtils = movingAgentUtils;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            long movingAgentId = long.Parse(dataMap.GetString("movingAgentId"));
            var datas = JsonSerializer.Deserialize<StartMissionRequest>(_movingAgentUtils.GetDatas(movingAgentId));
            _movingAgentUtils.RemoveMovingAgent(movingAgentId);
            await _missionService.StartMission(datas);
        }
    }

    public class AgentMovingOnMissionJobRunner : IAgentMovingOnMissionJobRunner
    {
        private readonly IMovingAgentUtils _movingAgentUtils;
        public AgentMovingOnMissionJobRunner(IMovingAgentUtils movingAgentUtils)
        {
            _movingAgentUtils = movingAgentUtils;   
        }

        public async Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId)
        {
            IScheduler scheduler = await factory.GetScheduler();

            var key = $"moveOnMissionJob{movingAgentId}";

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
            return JobBuilder.Create<AgentMovingOnMissionJob>()
                .WithIdentity($"moveOnMissionJob{movingAgentId}", "group1")
                .UsingJobData("movingAgentId", movingAgentId.ToString())
                .Build();
        }

        private ITrigger PrepareTrigger(long movingAgentId, DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"moveOnMissionTrigger{movingAgentId}", "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

    public interface IAgentMovingOnMissionJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId);
    }



}