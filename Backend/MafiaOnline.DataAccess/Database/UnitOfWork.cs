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
        }

        public IAgentRepository Agents { get; }
        public IBossRepository Bosses { get; }
        public IAgentForSaleRepository AgentsForSale { get; }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
