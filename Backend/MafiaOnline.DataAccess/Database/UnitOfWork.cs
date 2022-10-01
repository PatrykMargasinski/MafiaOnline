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
        IMissionRepository Missions { get; }
        IPerformingMissionRepository PerformingMissions { get; }
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
            PerformingMissions = new PerformingMissionRepository(context);
        }

        public IAgentRepository Agents { get; }
        public IAgentForSaleRepository AgentsForSale { get; }
        public IBossRepository Bosses { get; }
        public IPlayerRepository Players { get; }
        public IMissionRepository Missions { get; }
        public IPerformingMissionRepository PerformingMissions { get; }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
