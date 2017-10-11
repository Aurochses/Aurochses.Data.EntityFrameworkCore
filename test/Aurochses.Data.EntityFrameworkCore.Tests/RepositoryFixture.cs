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

            // data mapper
            DataMapper = new FakeDataMapper();

            // database
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(RepositoryFixture));

            UnitOfWork = new FakeUnitOfWork(
                dbContext => new FakeRepository(dbContext), 
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            UnitOfWork.FakeEntityRepository.Insert(new FakeEntity());
            UnitOfWork.Commit();

            UnitOfWork.FakeEntityRepository.Insert(new FakeEntity());
            UnitOfWork.Commit();
        }

        public FakeDataMapper DataMapper { get; }

        public FakeUnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}