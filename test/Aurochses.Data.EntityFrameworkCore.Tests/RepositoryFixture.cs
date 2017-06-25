using System;
using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class RepositoryFixture : IDisposable
    {
        public RepositoryFixture()
        {
            // automapper
            Mapper.Initialize(cfg => cfg.CreateMap<Entity<int>, FakeModel>());

            // database
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(RepositoryFixture));

            UnitOfWork = new FakeUnitOfWork(
                dbContext => new Repository<Entity<int>, int>(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            UnitOfWork.EntityRepository.Insert(new Entity<int>());
            UnitOfWork.Commit();
        }

        public FakeUnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}