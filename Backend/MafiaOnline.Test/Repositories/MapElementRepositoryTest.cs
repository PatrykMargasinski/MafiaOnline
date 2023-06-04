using AutoFixture;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using MafiaOnline.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.Test.Repositories
{
    public class MapElementRepositoryTest
    {

        private Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("mafia_db")
                .Options;

            var context = new DataContext(options);
            context.Database.EnsureDeleted();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        }

        //[Test]
        //public async Task Check_If_MapElement_Is_Created_When_Headquarters_Are_Added()
        //{
        //    var player = _fixture.Build<Player>().Without(x => x.Boss).Create();
        //    var boss = _fixture.Build<Boss>().Without(x=>x.MessageFromBosses).Without(x=>x.MessageToBosses).Without(x=>x.Player).Without(x=>x.Headquarters).Create();
        //    player.Boss = boss;
        //    var options = new DbContextOptionsBuilder<DataContext>()
        //        .UseInMemoryDatabase("mafia_db")
        //        .Options;

        //    using (var context = new DataContext(options))
        //    {
        //        IPlayerRepository repository = new PlayerRepository(context);
        //        repository.Create(player);
        //        IBossRepository bossRepository = new BossRepository(context);
        //        bossRepository.Create(boss);
        //        context.SaveChanges();
        //    }

        //    using (var context = new DataContext(options))
        //    {
        //        var headquarters = new Headquarters { BossId = boss.Id };
        //        var mapElement = new MapElement() { Type = MapElementType.Headquarters, X = 2, Y = 1 };
        //        headquarters.MapElement = mapElement;
        //        IHeadquartersRepository repository = new HeadquartersRepository(context);
        //        var headquartersReturned = await repository.GetAllAsync();
        //        repository.Create(headquarters);
        //        context.SaveChanges();
        //    }

        //    using (var context = new DataContext(options))
        //    {
        //        IHeadquartersRepository headquartersRepository = new HeadquartersRepository(context);
        //        IMapElementRepository mapElementRepository = new MapElementRepository(context);
        //        var headquartersReturned = await headquartersRepository.GetAllAsync();
        //        var mapElementsReturned = await mapElementRepository.GetAllAsync();
        //        Assert.AreEqual(mapElementsReturned.Count, headquartersReturned.Count);
        //    }
        //}
    }
}
