using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Aurochses.Data.EntityFrameworkCore.Tests.Fakes
{
    public static class EntityFrameworkMockHelpers
    {
        public static DbSet<TEntity> MockDbSet<TEntity>(List<TEntity> table)
            where TEntity : class
        {
            var dbSet = new Mock<DbSet<TEntity>>(MockBehavior.Strict);

            dbSet.As<IQueryable<TEntity>>().Setup(q => q.Provider).Returns(() => table.AsQueryable().Provider);
            dbSet.As<IQueryable<TEntity>>().Setup(q => q.Expression).Returns(() => table.AsQueryable().Expression);
            dbSet.As<IQueryable<TEntity>>().Setup(q => q.ElementType).Returns(() => table.AsQueryable().ElementType);
            dbSet.As<IQueryable<TEntity>>().Setup(q => q.GetEnumerator()).Returns(() => table.AsQueryable().GetEnumerator());

            return dbSet.Object;
        }
    }
}