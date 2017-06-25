using Microsoft.EntityFrameworkCore;
using System;

namespace Aurochses.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeUnitOfWork : UnitOfWork
    {
        public FakeUnitOfWork(
            Func<DbContext, IRepository<Entity<int>, int>> entityRepository,
            DbContextOptions dbContextOptions,
            string schemaName)
            : base(new FakeDbContext(dbContextOptions, schemaName))
        {
            RegisterRepository(entityRepository(DbContext));
        }

        public FakeUnitOfWork()
            : base(null)
        {
            RegisterRepository(new Repository<Entity<int>, int>(DbContext));
        }

        public IRepository<Entity<int>, int> EntityRepository => GetRepository<Entity<int>, int>();

        public void DoNotDispose()
        {
            Dispose(false);
        }
    }
}