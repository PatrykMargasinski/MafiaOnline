using MafiaOnline.DataAccess.Repositories;
using System.Threading.Tasks;

namespace MafiaOnline.DataAccess.Database
{
    public interface IUnitOfWork
    {
        void Commit();
        IAgentRepository Agents { get; }
        IAgentForSaleRepository AgentsForSale { get; }
        IMovingAgentRepository MovingAgents { get; }
        IBossRepository Bosses { get; }
        IMessageRepository Messages { get; }
        IMissionRepository Missions { get; }
        IMissionTemplateRepository MissionTemplates { get; }
        IPerformingMissionRepository PerformingMissions { get; }
        INameRepository Names { get; }
        IHeadquartersRepository Headquarters { get; }
        IMapElementRepository MapElements { get; }
        IExposedMapElementRepository ExposedMapElements { get; }
        IAmbushRepository Ambushes { get; }
        INewsRepository News { get; }
        IVBossRepository VBosses { get; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Agents = new AgentRepository(context);
            AgentsForSale = new AgentForSaleRepository(context);
            MovingAgents = new MovingAgentRepository(context);
            Bosses = new BossRepository(context);
            Missions = new MissionRepository(context);
            MissionTemplates = new MissionTemplateRepository(context);
            PerformingMissions = new PerformingMissionRepository(context);
            Messages = new MessageRepository(context);
            Names = new NameRepository(context);
            Headquarters = new HeadquartersRepository(context);
            MapElements = new MapElementRepository(context);
            ExposedMapElements = new ExposedMapElementRepository(context);
            Ambushes = new AmbushRepository(context);
            News = new NewsRepository(context);
            VBosses = new VBossRepository(context);
        }

        public IAgentRepository Agents { get; }
        public IAgentForSaleRepository AgentsForSale { get; }
        public IMovingAgentRepository MovingAgents { get; }
        public IBossRepository Bosses { get; }
        public IMissionRepository Missions { get; }
        public IMissionTemplateRepository MissionTemplates { get; }
        public IMessageRepository Messages { get; }
        public IPerformingMissionRepository PerformingMissions { get; }
        public INameRepository Names { get; }
        public IHeadquartersRepository Headquarters { get; }
        public IMapElementRepository MapElements { get; }
        public IExposedMapElementRepository ExposedMapElements { get; }
        public IAmbushRepository Ambushes { get; }
        public INewsRepository News { get; }
        public IVBossRepository VBosses { get; }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
