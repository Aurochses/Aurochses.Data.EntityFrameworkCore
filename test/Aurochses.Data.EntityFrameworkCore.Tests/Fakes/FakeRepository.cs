using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeRepository : Repository<FakeEntity, int>
    {
        public FakeRepository(DbContext dbContext)
            : base(dbContext)
        {

        }

        public DbSet<FakeEntity> ProtectedDbSet => DbSet;

        public IQueryable<FakeEntity> ProtectedQuery(QueryParameters<FakeEntity, int> queryParameters = null)
        {
            return Query(queryParameters);
        }

        public IQueryable<TModel> ProtectedQuery<TModel>(IDataMapper dataMapper, QueryParameters<FakeEntity, int> queryParameters = null)
        {
            return Query<TModel>(dataMapper, queryParameters);
        }
    }
}