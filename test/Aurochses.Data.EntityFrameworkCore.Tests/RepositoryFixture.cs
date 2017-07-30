using System;
using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class RepositoryFixture : IDisposable
    {
        public RepositoryFixture()
        {
            // automapper
            AutoMapper.Mapper.Initialize(cfg => cfg.CreateMap<Entity<int>, FakeModel>());

            // mapper
            Mapper = new FakeMapper();

            // database
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(RepositoryFixture));

            UnitOfWork = new FakeUnitOfWork(
                dbContext => new Repository<Entity<int>, int>(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            UnitOfWork.EntityRepository.Insert(new Entity<int>());
            UnitOfWork.Commit();

            UnitOfWork.EntityRepository.Insert(new Entity<int>());
            UnitOfWork.Commit();
        }

        public FakeMapper Mapper { get; }

        public FakeUnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}