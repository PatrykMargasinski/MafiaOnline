using AutoMapper;
using MafiaAPI.Jobs;
using MafiaOnline.BusinessLogic.Const;
using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Factories;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.BusinessLogic.Validators;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Services
{

    public interface IAmbushService
    {
        Task MoveToArrangeAmbush(ArrangeAmbushRequest request);
        Task ArrangeAmbush(ArrangeAmbushRequest request);
        Task<AmbushDTO> GetAmbushDetailsByMapElementId(long mapElementId);
        Task CancelAmbush(CancelAmbushRequest request);
        Task AttackAmbush(AttackAmbushRequest request);
        Task MoveToAttackAmbush(AttackAmbushRequest request);

    }

    public class AmbushService : IAmbushService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAmbushValidator _ambushValidator;
        private readonly IAmbushFactory _ambushFactory;
        private readonly IArrangeAmbushJobRunner _arrangeAmbushJobRunner;
        private readonly ISchedulerFactory _scheduler;
        private readonly IReporter _reporter;
        private readonly IMapper _mapper;
        private readonly IAttackAmbushJobRunner _attackAmbushJobRunner;
        private readonly IAgentUtils _agentUtils;

        public AmbushService(IUnitOfWork unitOfWork, IAmbushValidator ambushValidator, IAmbushFactory ambushFactory, ISchedulerFactory scheduler, 
            IReporter reporter, IArrangeAmbushJobRunner arrangeAmbushJobRunner, IMapper mapper, IAttackAmbushJobRunner attackAmbushJobRunner, IAgentUtils agentUtils)
        {
            _unitOfWork = unitOfWork;
            _ambushValidator = ambushValidator;
            _ambushFactory = ambushFactory;
            _scheduler = scheduler;
            _reporter = reporter;
            _arrangeAmbushJobRunner = arrangeAmbushJobRunner;
            _mapper = mapper;
            _agentUtils = agentUtils;
            _attackAmbushJobRunner = attackAmbushJobRunner;
        }

        public async Task<AmbushDTO> GetAmbushDetailsByMapElementId(long mapElementId)
        {
            var ambush = await _unitOfWork.Ambushes.GetByMapElementIdAsync(mapElementId);
            return _mapper.Map<AmbushDTO>(ambush);
        }

        public async Task MoveToArrangeAmbush(ArrangeAmbushRequest request)
        {
            await _ambushValidator.ValidateMoveToArrangeAmbush(request);
            var movingAgent = await _ambushFactory.CreateAgentMovingOnAmbush(request);
            _unitOfWork.MovingAgents.Create(movingAgent);
            _unitOfWork.Commit();
            await _arrangeAmbushJobRunner.Start(_scheduler, movingAgent.ArrivalTime, movingAgent.Id);
        }


        public async Task CancelAmbush(CancelAmbushRequest request)
        {
            await _ambushValidator.ValidateCancelAmbush(request);
            var ambush = await _unitOfWork.Ambushes.GetByMapElementIdAsync(request.MapElementId);
            var agent = await _unitOfWork.Agents.GetByIdAsync(ambush.AgentId);
            agent.StateIdEnum = AgentState.Active;
            _unitOfWork.MapElements.DeleteById(request.MapElementId);
            _unitOfWork.Commit();
        }

        public async Task ArrangeAmbush(ArrangeAmbushRequest request)
        {
            var agent = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var boss = await _unitOfWork.Bosses.GetByIdAsync(agent.BossId.Value);

            try
            {
            await _ambushValidator.ValidateArrangeAmbush(request);
            }
            catch(Exception ex)
            {
                string messageContent;
                messageContent = $"Agent {agent.FullName ?? ""} wanted to arrange ambush";
                messageContent += ", but there are reasons why he couldn't:\n";
                messageContent += $"{ex.Message}\n\n";
                messageContent += "The agent returns to the headquarters";
                await _reporter.CreateReport(boss.Id, "The agent returns to the headquarters", messageContent);
                agent.StateIdEnum = AgentState.Active;
                _unitOfWork.Commit();
                return;
            }

            var ambush = new Ambush()
            {
                Agent = agent,
                Boss = boss
            };

            var mapElement = new MapElement()
            {
                X = request.Point.X,
                Y = request.Point.Y,
                Boss = boss,
                Type = MapElementType.Ambush,
                Ambush = ambush,
                Hidden = true
            };

            agent.StateIdEnum = AgentState.Ambushing;

            _unitOfWork.MapElements.Create(mapElement);
            _unitOfWork.Commit();
        }


        public async Task MoveToAttackAmbush(AttackAmbushRequest request)
        {
            await _ambushValidator.ValidateMoveOnAttackAmbush(request);
            var movingAgent = await _ambushFactory.CreateMovingAgentForAttackingAmbush(request);
            _unitOfWork.MovingAgents.Create(movingAgent);
            _unitOfWork.Commit();
            await _attackAmbushJobRunner.Start(_scheduler, movingAgent.ArrivalTime, movingAgent.Id);
        }

        public async Task AttackAmbush(AttackAmbushRequest request)
        {
            var attacker = await _unitOfWork.Agents.GetByIdAsync(request.AgentId);
            var ambush = await _unitOfWork.Ambushes.GetByMapElementIdAsync(request.MapElementId);
            var defender = await _unitOfWork.Agents.GetByIdAsync(ambush.AgentId);
            try
            {
                await _ambushValidator.ValidateAttackAmbush(request);
                var shootoutResult = await _agentUtils.Shootout(attacker.Id, defender.Id, attacker.Id);
                var reportForAttacker = "Your agent's attacked ambush.\n";
                var reportForDefender = "Your ambush has been attacked.\n";

                //attacker wins
                if (shootoutResult.WinnerAgentId == attacker.Id)
                {
                    reportForAttacker += "\nYour agent won the shootout. Ambush has been destroyed.\nAgent returns the the headquarters.";
                    reportForDefender += "\nYour agent lost the shootout. Your ambush has been destroyed.\nAgent returns the the headquarters.";
                    _unitOfWork.Ambushes.DeleteById(ambush.Id);
                    _unitOfWork.MapElements.DeleteById(ambush.MapElementId);
                    attacker.StateIdEnum = AgentState.Active;
                    defender.StateIdEnum = AgentState.Active;
                    return;
                }
                //defender wins
                else
                {
                    reportForAttacker += "\nYour agent lost the shootout. The ambush still exists.\nAgent returns the the headquarters.";
                    reportForDefender += "\nYour agent won the shootout. The ambush still exists.";
                    attacker.StateIdEnum = AgentState.Active;
                }
                await _reporter.CreateReport(attacker.BossId.Value, "Shootout", reportForAttacker);
                await _reporter.CreateReport(defender.BossId.Value, "Shootout", reportForDefender);
            }
            catch (Exception ex)
            {
                string messageContent;
                messageContent = $"Agent {attacker.FullName ?? ""} wanted to attack ambush, but there are reasons why he couldn't:\n";
                messageContent += $"{ex.Message}\n\n";
                messageContent += "The agent returns to the headquarters";
                await _reporter.CreateReport(attacker.BossId.Value, "The agent returns to the headquarters", messageContent);
                attacker.StateIdEnum = AgentState.Active;
                _unitOfWork.Commit();
                return;
            }
        }
    }
}
