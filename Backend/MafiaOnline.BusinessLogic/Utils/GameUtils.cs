using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IGameUtils
    {
        Task ResetGame();
    }

    public class GameUtils : IGameUtils
    {
        public GameUtils(IUnitOfWork unitOfWork, ILogger<GameUtils> logger, ISchedulerFactory scheduler)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _scheduler = scheduler;
        }

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchedulerFactory _scheduler;
        private readonly ILogger<GameUtils> _logger;

        public async Task ResetGame()
        {
            var scheduler = await _scheduler.GetScheduler();
            var jobKeysToRemove = new List<JobKey>();

            //reseting bosses
            var bosses = await _unitOfWork.Bosses.GetAllAsync();
            foreach(var boss in bosses)
            {
                boss.Money = 5000;
            }

            //removing all performing missions instances
            var performingMissions = await _unitOfWork.PerformingMissions.GetAllAsync();
            jobKeysToRemove.AddRange(performingMissions.Select(x => new JobKey(x.JobKey)));
            _unitOfWork.PerformingMissions.DeleteByIds(performingMissions.Select(x => x.Id).ToArray());

            //removing all moving agent instances
            var movingAgents = await _unitOfWork.MovingAgents.GetAllAsync();
            jobKeysToRemove.AddRange(movingAgents.Select(x => new JobKey(x.JobKey)));
            _unitOfWork.MovingAgents.DeleteByIds(performingMissions.Select(x => x.Id).ToArray());

            await scheduler.DeleteJobs(jobKeysToRemove);

            //removing all not from family agents
            var agents = await _unitOfWork.Agents.GetAllAsync();
            var agentsToDelete = agents.Where(x => !x.IsFromBossFamily).ToList();
            _unitOfWork.Agents.DeleteByIds(agentsToDelete.Select(x => x.Id).ToArray());

            //reseting all from family agents
            foreach(var agent in agents.Where(x=>x.IsFromBossFamily))
                agent.StateIdEnum = AgentState.Active;

            //removing all non-headquarters map elements
            var mapElements = await _unitOfWork.MapElements.GetAllAsync();
            mapElements = mapElements.Where(x => x.Type != MapElementType.Headquarters).ToList();
            _unitOfWork.MapElements.DeleteByIds(mapElements.Select(x => x.Id).ToArray());
            _unitOfWork.Commit();
        }
    }
}
