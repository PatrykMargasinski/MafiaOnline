using MafiaOnline.DataAccess.Repositories;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Database
{
    public interface IUnitOfWork
    {
        void Commit();
        IAgentRepository Agents { get; }
        IAgentForSaleRepository AgentsForSale { get; }
        IBossRepository Bosses { get; }
        IPlayerRepository Players { get; }
        IMessageRepository Messages { get; }
        IMissionRepository Missions { get; }
        IMissionTemplateRepository MissionTemplates { get; }
        IPerformingMissionRepository PerformingMissions { get; }
        INameRepository Names { get; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Agents = new AgentRepository(context);
            AgentsForSale = new AgentForSaleRepository(context);
            Bosses = new BossRepository(context);
            Players = new PlayerRepository(context);
            Missions = new MissionRepository(context);
            MissionTemplates = new MissionTemplateRepository(context);
            PerformingMissions = new PerformingMissionRepository(context);
            Messages = new MessageRepository(context);
            Names = new NameRepository(context);
            
        }

        public IAgentRepository Agents { get; }
        public IAgentForSaleRepository AgentsForSale { get; }
        public IBossRepository Bosses { get; }
        public IPlayerRepository Players { get; }
        public IMissionRepository Missions { get; }
        public IMissionTemplateRepository MissionTemplates { get; }
        public IMessageRepository Messages { get; }
        public IPerformingMissionRepository PerformingMissions { get; }
        public INameRepository Names { get; }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
