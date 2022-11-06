using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Logging;
using System;
using System.Threading.Tasks;
using static Quartz.Logging.OperationName;

namespace MafiaAPI.Jobs
{

    [DisallowConcurrentExecution]
    public class MissionRefreshJob : IJob
    {
        private readonly ILogger<MissionRefreshJob> _logger;
        private readonly IMissionService _missionService;
        public MissionRefreshJob(
            ILogger<MissionRefreshJob> logger,
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
            await _missionService.RefreshMissions();
            await _missionService.ScheduleRefreshMissionsJob();
        }
    }

    public class MissionRefreshJobRunner : IMissionRefreshJobRunner
    {
        public async Task Start(ISchedulerFactory factory, DateTime finishTime)
        {
            IScheduler scheduler = await factory.GetScheduler();

            JobKey jobKey = new("missionRefreshJob", "group1");
            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.DeleteJob(jobKey);
            }

            IJobDetail newJob = PrepareJobDetail();
            ITrigger trigger = PrepareTrigger(finishTime);

            await scheduler.ScheduleJob(newJob, trigger);
            await scheduler.Start();
        }

        private IJobDetail PrepareJobDetail()
        {
            return JobBuilder.Create<MissionRefreshJob>()
                .WithIdentity("missionRefreshJob", "group1")
                .Build();
        }

        private ITrigger PrepareTrigger(DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity("missionRefreshTrigger", "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

    public interface IMissionRefreshJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime);
    }



}