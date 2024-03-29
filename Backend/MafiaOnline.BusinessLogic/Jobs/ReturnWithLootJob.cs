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
        private readonly IAgentActionsService _movingAgentService;
        private readonly IMovingAgentUtils _movingAgentUtils;
        public ReturnWithLootJob(
            ILogger<ReturnWithLootJob> logger,
            IAgentActionsService movingAgentService,
            IMovingAgentUtils movingAgentUtils
            )
        {
            _logger = logger;
            _movingAgentService = movingAgentService;
            _movingAgentUtils = movingAgentUtils;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            long movingAgentId = long.Parse(dataMap.GetString("movingAgentId"));
            await _movingAgentService.MakeStepMovingWithLoot(movingAgentId);
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
            var movingAgent = await _movingAgentUtils.GetMovingAgent(movingAgentId);
            ITrigger trigger = PrepareTrigger(movingAgent, finishTime);

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

        private ITrigger PrepareTrigger(MovingAgent movingAgent, DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"returnWithLootTrigger{movingAgent.Id}", "group1")
                .StartAt(finishTime)
                .WithSimpleSchedule(
                    x => x
                    .WithIntervalInSeconds(MapConsts.SECONDS_TO_MAKE_ONE_STEP)
                    .WithMisfireHandlingInstructionFireNow()
                    .WithRepeatCount(movingAgent.Path.Length)
                 )
                .Build();
        }
    }

    public interface IReturnWithLootJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime, long movingAgentId);
    }



}