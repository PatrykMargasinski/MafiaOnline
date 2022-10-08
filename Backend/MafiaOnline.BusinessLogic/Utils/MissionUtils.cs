using MafiaOnline.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface IMissionUtils
    {
        int CalculateAgentSuccessChance(Agent agent, Mission mission);
        bool IsMissionSuccessfullyCompleted(Agent agent, Mission mission);
    }

    public class MissionUtils : IMissionUtils
    {
        /// <summary>
        /// Calculates how big the probability of mission success is
        /// </summary>
        public int CalculateAgentSuccessChance(Agent agent, Mission mission)
        {
            var agentPower = Math.Round(agent.Strength * mission.StrengthPercentage / 100.0 +
            agent.Dexterity * mission.DexterityPercentage / 100.0 +
            agent.Intelligence * mission.IntelligencePercentage / 100.0);
            var chance = (int)(agentPower / mission.DifficultyLevel * 100);
            if (chance > 100) chance = 100;
            else if (chance < 10) chance = 10;
            return chance;
        }

        /// <summary>
        /// Checks if mission is successfully completed
        /// </summary>
        public bool IsMissionSuccessfullyCompleted(Agent agent, Mission mission)
        {
            Random random = new();
            var value = random.Next(101);
            return value <= CalculateAgentSuccessChance(agent, mission);
        }
    }
}
