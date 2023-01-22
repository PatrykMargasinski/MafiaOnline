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
    public class RemoveNotActivatedPlayerJob : IJob
    {
        private readonly ILogger<RemoveNotActivatedPlayerJob> _logger;
        private readonly IPlayerService _playerService;
        public RemoveNotActivatedPlayerJob(
            ILogger<RemoveNotActivatedPlayerJob> logger,
            IPlayerService playerService
            )
        {
            _logger = logger;
            _playerService = playerService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            long playerId = long.Parse(dataMap.GetString("playerId"));
            var request = new DeleteAccountRequest()
            {
                PlayerId = playerId
            };
            _logger.LogDebug($"Removed player with id {playerId}");
            await _playerService.DeleteAccount(request, true);
        }
    }

    public class RemoveNotActivatedPlayerJobRunner : IRemoveNotActivatedPlayerJobRunner
    {
        private readonly IPlayerUtils _playerUtils;
        public RemoveNotActivatedPlayerJobRunner(IPlayerUtils playerUtils)
        {
            _playerUtils = playerUtils;   
        }

        public async Task Start(ISchedulerFactory factory, DateTime deletionTime, long playerId)
        {
            IScheduler scheduler = await factory.GetScheduler();

            var key = $"removeNotActivatedPlayer{playerId}";
            await _playerUtils.SetJobKey(playerId, key);

            JobKey jobKey = new(key, "group1");
            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.DeleteJob(jobKey);
            }

            IJobDetail newJob = PrepareJobDetail(playerId);
            ITrigger trigger = PrepareTrigger(playerId, deletionTime);

            await scheduler.ScheduleJob(newJob, trigger);
            await scheduler.Start();
        }

        private IJobDetail PrepareJobDetail(long playerId)
        {
            return JobBuilder.Create<RemoveNotActivatedPlayerJob>()
                .WithIdentity($"removeNotActivatedPlayer{playerId}", "group1")
                .UsingJobData("playerId", playerId.ToString())
                .Build();
        }

        private ITrigger PrepareTrigger(long playerId, DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"removeNotActivatedPlayerTrigger{playerId}", "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

    public interface IRemoveNotActivatedPlayerJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime, long playerId);
    }



}