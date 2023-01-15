﻿using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.Test.Database
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        public DataContext context;

        public FakeUnitOfWork()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("mafia_db")
                .Options;

            context = new DataContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            Agents = new AgentRepository(context);
            AgentsForSale = new AgentForSaleRepository(context);
            Bosses = new BossRepository(context);
            Players = new PlayerRepository(context);
            Messages = new MessageRepository(context);
            Missions = new MissionRepository(context);
            MissionTemplates = new MissionTemplateRepository(context);
            PerformingMissions = new PerformingMissionRepository(context);
            Names = new NameRepository(context);
        }

        public IAgentRepository Agents { get; }

        public IAgentForSaleRepository AgentsForSale { get; }

        public IBossRepository Bosses { get; }

        public IPlayerRepository Players { get; }

        public IMessageRepository Messages { get; }

        public IMissionRepository Missions { get; }

        public IMissionTemplateRepository MissionTemplates { get; }

        public IPerformingMissionRepository PerformingMissions { get; }

        public INameRepository Names { get; }

        public void Commit()
        {
            context.SaveChanges();
        }
    }
}