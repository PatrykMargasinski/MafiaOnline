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
            await _agentService.RefreshAgents();
            await _agentService.ScheduleRefreshAgentsJob();
        }
    }

    public class AgentRefreshJobRunner : IAgentRefreshJobRunner
    {
        public async Task Start(ISchedulerFactory factory, DateTime finishTime)
        {
            IScheduler scheduler = await factory.GetScheduler();

            JobKey jobKey = new("agentRefreshJob", "group1");
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