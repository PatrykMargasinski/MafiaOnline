using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.BusinessLogic.Services;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IAgentUtils
    {
        Task<ShootoutResult> Shootout(long agent1Id, long agent2Id, long? agentWithAdvantageId = null);
    }

    public class AgentUtils : IAgentUtils
    {
        private readonly IRandomizer _randomizer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AgentUtils> _logger;

        public AgentUtils(IRandomizer randomizer, IUnitOfWork unitOfWork, ILogger<AgentUtils> logger)
        {
            _randomizer = randomizer;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ShootoutResult> Shootout(long agent1Id, long agent2Id, long? agentWithAdvantageId = null)
        {
            var result = new ShootoutResult();
            var agents = (await _unitOfWork.Agents.GetByIdsAsync(new long[] {agent1Id, agent2Id})).ToList();

            result.Report = $"There was a shootout between agents {agents[0].FullName} and {agents[1].FullName}\n";
            _logger.LogDebug($"Shootout beetween {agent1Id} {agent2Id} with advantage: {agentWithAdvantageId ?? 0}");
            List<AgentInShootout> agentsInShootout = agents.Select
            (
                x =>
                new AgentInShootout()
                {
                    Id = x.Id,
                    Attributes = new int[] { x.Strength, x.Dexterity, x.Intelligence },
                    FullName = x.FullName,
                    Points = 0
                }
            ).ToList();

            if (agentWithAdvantageId == null)
            {
                foreach (var agent in agentsInShootout)
                    agent.Power = agent.Attributes.Max();
            }
            else
            {
                var agentWithAdvantage = agentsInShootout.FirstOrDefault(x => x.Id == agentWithAdvantageId.Value);
                result.Report += $"Agent {agentWithAdvantage.FullName} has advantage in this shootout\n";
                var attributeCompared = agentWithAdvantage.Attributes.ToList().IndexOf(agentWithAdvantage.Attributes.Max());

                foreach (var agent in agentsInShootout)
                    agent.Power = agent.Attributes[attributeCompared];
            }

            for (int i=0; i<10; i++)
            {
                foreach(var agent in agentsInShootout)
                {
                    var randomNumber = _randomizer.Next(12);
                    if (agent.Power >= randomNumber)
                        agent.Points++;
                }
            }
            _logger.LogDebug($"Points in shootout: \n" + string.Join(",", agentsInShootout.Select(x=>$"Agent: {x.Id} Points: {x.Points}").ToArray()));
            var winner = agentsInShootout.OrderBy(_ => _randomizer.Next()).FirstOrDefault(x => x.Points == agentsInShootout.Select(x => x.Points).Max());
            result.WinnerAgentId = winner.Id;
            result.Report += $"\nEventually, agent {winner.FullName} won";
            return result;
        }
    }
}
