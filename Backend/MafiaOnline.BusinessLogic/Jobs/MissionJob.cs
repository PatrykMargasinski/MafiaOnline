using MafiaOnline.BusinessLogic.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MafiaAPI.Jobs
{

    [DisallowConcurrentExecution]
    public class MissionJob : IJob
    {
        private readonly ILogger<MissionJob> _logger;
        private readonly IMissionService _missionService;
        public MissionJob(
            ILogger<MissionJob> logger,
            IMissionService missionService
            )
        {
            _logger = logger;
            _missionService = missionService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            long pmId = Int32.Parse(dataMap.GetString("pmId"));
            await _missionService.EndMission(pmId);
        }

        public async static Task Start(ISchedulerFactory factory, long pmId, DateTime finishTime)
        {
            IScheduler scheduler = await factory.GetScheduler();
            IJobDetail job = PrepareJobDetail(pmId);
            ITrigger trigger = PrepareTrigger(finishTime, pmId);

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }

        private static IJobDetail PrepareJobDetail(long pmId)
        {
            return JobBuilder.Create<MissionJob>()
                .WithIdentity("missionJob" + pmId, "group1")
                .UsingJobData("pmId", pmId.ToString())
                .Build();
        }

        private static ITrigger PrepareTrigger(DateTime finishTime, long pmId)
        {
            return TriggerBuilder.Create()
                .WithIdentity("missionTrigger" + pmId, "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

}