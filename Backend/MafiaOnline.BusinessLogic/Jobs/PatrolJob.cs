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
    public class PatrolJob : IJob
    {
        private readonly ILogger<PatrolJob> _logger;
        private readonly IAgentService _agentService;
        private readonly IMovingAgentUtils _movingAgentUtils;
        public PatrolJob(
            ILogger<PatrolJob> logger,
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
            await _agentService.MakeStepDuringPatrolling(movingAgentId);
        }
    }

    public class PatrolJobRunner : IPatrolJobRunner
    {
        private readonly IMovingAgentUtils _movingAgentUtils;
        public PatrolJobRunner(IMovingAgentUtils movingAgentUtils)
        {
            _movingAgentUtils = movingAgentUtils;   
        }

        public async Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId)
        {
            IScheduler scheduler = await factory.GetScheduler();

            var key = $"patrolJob{movingAgentId}";

            await _movingAgentUtils.SetJobKey(movingAgentId, key);

            JobKey jobKey = new(key, "group1");
            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.DeleteJob(jobKey);
            }
            var movingAgent = await _movingAgentUtils.GetMovingAgent(movingAgentId);
            IJobDetail newJob = PrepareJobDetail(movingAgentId);
            ITrigger trigger = PrepareTrigger(movingAgent, finishTime);

            await scheduler.ScheduleJob(newJob, trigger);
            await scheduler.Start();
        }

        private IJobDetail PrepareJobDetail(long movingAgentId)
        {
            return JobBuilder.Create<PatrolJob>()
                .WithIdentity($"patrolJob{movingAgentId}", "group1")
                .UsingJobData("movingAgentId", movingAgentId.ToString())
                .Build();
        }

        private ITrigger PrepareTrigger(MovingAgent movingAgent, DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"patrolTrigger{movingAgent.Id}", "group1")
                .StartAt(finishTime)
                .WithSimpleSchedule(
                    x => x
                    .WithIntervalInSeconds(MapConsts.SECONDS_TO_MAKE_ONE_STEP)
                    .WithRepeatCount(movingAgent.Path.Length)
                    .WithMisfireHandlingInstructionIgnoreMisfires()
                )
                .Build();
        }
    }

    public interface IPatrolJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId);
    }



}