using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using MafiaOnline.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.Test.Repositories
{
    public class CrudRepositoryTest
    {
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("mafia_db")
                .Options;

            var context = new DataContext(options);
            context.Database.EnsureDeleted();

        }

        [Test]
        public async Task Create_Method_Should_Save_Entities_And_GetAll_Should_Return_Them()
        {
            var name1 = new Name { Text = "text1", Type = NameType.FirstName };
            var name2 = new Name { Text = "text2", Type = NameType.LastName };

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("mafia_db")
                .Options;

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                repository.Create(name1);
                repository.Create(name2);
                context.SaveChanges();
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                var names = await repository.GetAllAsync();
                Assert.AreEqual(2, names.Count);
                Assert.Contains("text1", names.Select(x => x.Text).ToList());
                Assert.Contains("text2", names.Select(x => x.Text).ToList());
            }
        }

        [Test]
        public async Task Delete_Should_Remove_Elements()
        {
            var name1 = new Name { Text = "text1", Type = NameType.FirstName };
            var name2 = new Name { Text = "text2", Type = NameType.LastName };
            var name3 = new Name { Text = "text3", Type = NameType.LastName };

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("mafia_db")
                .Options;

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                repository.Create(name1);
                repository.Create(name2);
                repository.Create(name3);
                context.SaveChanges();
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                repository.DeleteById(name1.Id);
                context.SaveChanges();
                var names = await repository.GetAllAsync();
                Assert.AreEqual(2, names.Count);
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                repository.DeleteByIds(new long[] { name2.Id, name3.Id });
                context.SaveChanges();
                var names = await repository.GetAllAsync();
                Assert.AreEqual(0, names.Count);
            }
        }


        [Test]
        public async Task Check_If_GetById_Method_Works_Properly()
        {
            var name1 = new Name { Text = "text1", Type = NameType.FirstName };
            var name2 = new Name { Text = "text2", Type = NameType.LastName };
            var name3 = new Name { Text = "text3", Type = NameType.LastName };

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("mafia_db")
                .Options;

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                repository.Create(name1);
                repository.Create(name2);
                repository.Create(name3);
                context.SaveChanges();
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                var name = await repository.GetByIdAsync(name1.Id);
                Assert.AreEqual("text1", name.Text);
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                var names = await repository.GetByIdsAsync(new long[] { name1.Id, name2.Id });
                Assert.Contains("text1", names.Select(x => x.Text).ToList());
                Assert.Contains("text2", names.Select(x => x.Text).ToList());
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                var names = await repository.GetAllAsync();
                Assert.Contains("text1", names.Select(x => x.Text).ToList());
                Assert.Contains("text2", names.Select(x => x.Text).ToList());
                Assert.Contains("text3", names.Select(x => x.Text).ToList());
            }
        }

        [Test]
        public async Task Test_If_Find_Method_Works_Properly()
        {
            var name1 = new Name { Text = "text1", Type = NameType.FirstName };
            var name2 = new Name { Text = "text2", Type = NameType.LastName };
            var name3 = new Name { Text = "text3", Type = NameType.LastName };

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("mafia_db")
                .Options;

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                repository.Create(name1);
                repository.Create(name2);
                repository.Create(name3);
                context.SaveChanges();
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                var filterObject = new Name() { Text = "text1" };
                var name = (await repository.FindAsync(filterObject)).FirstOrDefault();
                Assert.AreEqual("text1", name.Text);
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                var filterObject = new Name() { Text = "text11" };
                var name = (await repository.FindAsync(filterObject)).FirstOrDefault();
                Assert.IsNull(name);
            }

            using (var context = new DataContext(options))
            {
                ICrudRepository<Name> repository = new NameRepository(context);
                var filterObject = new Name() { Type = NameType.LastName };
                var names = await repository.FindAsync(filterObject);
                Assert.AreEqual(2, names.Count);
            }
        }
    }
}
