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
    public class RemoveResetPasswordCodeJob : IJob
    {
        private readonly ILogger<RemoveResetPasswordCodeJob> _logger;
        private readonly IPlayerService _playerService;
        public RemoveResetPasswordCodeJob(
            ILogger<RemoveResetPasswordCodeJob> logger,
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
            _logger.LogDebug($"Removed player with id {playerId}");
            await _playerService.RemoveResetPasswordCode(playerId);
        }
    }

    public class RemoveResetPasswordCodeJobRunner : IRemoveResetPasswordCodeJobRunner
    {
        private readonly IPlayerUtils _playerUtils;
        public RemoveResetPasswordCodeJobRunner(IPlayerUtils playerUtils)
        {
            _playerUtils = playerUtils;
        }

        public async Task Start(ISchedulerFactory factory, DateTime deletionTime, long playerId)
        {
            IScheduler scheduler = await factory.GetScheduler();

            var key = $"resetPasswordCode{playerId}";

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
            return JobBuilder.Create<RemoveResetPasswordCodeJob>()
                .WithIdentity($"resetPasswordCode{playerId}", "group1")
                .UsingJobData("playerId", playerId.ToString())
                .Build();
        }

        private ITrigger PrepareTrigger(long playerId, DateTime finishTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity($"resetPasswordCodeTrigger{playerId}", "group1")
                .StartAt(finishTime)
                .Build();
        }
    }

    public interface IRemoveResetPasswordCodeJobRunner
    {
        Task Start(ISchedulerFactory factory, DateTime finishTime, long playerId);
    }



}