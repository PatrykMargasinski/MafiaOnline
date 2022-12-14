
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Factories
{


    public interface IMissionFactory
    {
        Task<Mission> CreateByMissionTemplate(MissionTemplate template);
        Mission Create(string name = "Mission with no name", int? difficultyLevel = null, double? duration = null, int? loot = null,
        int? strengthPercentage = null, int? dexterityPercentage = null, int? intelligencePercentage = null);
    }

    public class MissionFactory : IMissionFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRandomizer _randomizer;

        public MissionFactory(IUnitOfWork unitOfWork, IRandomizer randomizer)
        {
            _unitOfWork = unitOfWork;
            _randomizer = randomizer;
        }

        public async Task<Mission> CreateByMissionTemplate(MissionTemplate template)
        {
            if(template == null)
                template = await _unitOfWork.MissionTemplates.GetRandomAsync();
            var mission = new Mission()
            {
                Name = template.Name,
                DifficultyLevel = _randomizer.Next(template.MinDifficulty, template.MaxDifficulty),
                Duration = _randomizer.Next(template.MinDuration, template.MaxDuration),
                Loot = _randomizer.Next(template.MinLoot, template.MaxLoot),
                StrengthPercentage = template.StrengthPercentage,
                DexterityPercentage = template.DexterityPercentage,
                IntelligencePercentage = template.IntelligencePercentage
            };
            return mission;
        }

        public Mission Create(string name = "Mission with no name", int? difficultyLevel = null, double? duration = null, int? loot = null, 
            int? strengthPercentage = null, int? dexterityPercentage = null, int? intelligencePercentage = null)
        {
            int strPerc, dexPerc, intPerc;
            if(!strengthPercentage.HasValue || !dexterityPercentage.HasValue || !intelligencePercentage.HasValue)
            {
                strPerc = _randomizer.Next(0, 20);
                dexPerc = _randomizer.Next(0, 20 - strPerc);
                intPerc = 20 - strPerc - dexPerc;
                strPerc *= 5; dexPerc *= 5; intPerc *= 5;
            }
            else
            {
                strPerc = strengthPercentage.Value; dexPerc = dexterityPercentage.Value; intPerc = intelligencePercentage.Value;
            }
            var mission = new Mission()
            {
                Name = name,
                DifficultyLevel = difficultyLevel ?? _randomizer.Next(1, 10),
                Duration = duration ?? _randomizer.Next(1, 10)*10,
                Loot = loot ?? _randomizer.Next(1, 10) * 100,
                StrengthPercentage = strPerc,
                DexterityPercentage = dexPerc,
                IntelligencePercentage = intPerc
            };
            return mission;
        }
    }
}
