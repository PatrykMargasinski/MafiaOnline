using MafiaOnline.BusinessLogic.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Logging;
using System;
using System.Threading.Tasks;

namespace MafiaAPI.Jobs
{

    [DisallowConcurrentExecution]
    public class AgentRefreshJob : IJob
    {
        private readonly ILogger<AgentRefreshJob> _logger;
        private readonly IAgentService _agentService;
        public AgentRefreshJob(
            ILogger<AgentRefreshJob> logger,
            IAgentService agentService
            )
        {
            _logger = logger;
            _agentService = agentService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            await _agentService.StartRefreshAgentsJob();
        }
    }

    public class AgentRefreshJobRunner : IAgentRefreshJobRunner
    {
        public async Task Start(ISchedulerFactory factory, DateTime finishTime)
        {
            IScheduler scheduler = await factory.GetScheduler();
            IJobDetail job = PrepareJobDetail();
            ITrigger trigger = PrepareTrigger(finishTime);

            if (await scheduler.CheckExists(trigger.Key))
            {
                await scheduler.UnscheduleJob(trigger.Key);
            }

            if (await scheduler.CheckExists(job.Key))
            {
                await scheduler.DeleteJob(job.Key);
            }

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }

        private IJobDetail PrepareJobDetail()
        {
            return JobBuilder.Create<AgentRefreshJob>()
                .WithIdentity("agentRefreshJob", "group1")
                .Build();
        }

        private ITrigger PrepareTrigger(DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity("agentRefreshTrigger", "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

    public interface IAgentRefreshJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime);
    }



}