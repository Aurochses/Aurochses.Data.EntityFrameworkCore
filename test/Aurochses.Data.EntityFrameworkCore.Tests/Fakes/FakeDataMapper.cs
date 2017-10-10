using System.Linq;
using AutoMapper.QueryableExtensions;

namespace Aurochses.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeDataMapper : IDataMapper
    {
        public IQueryable<TDestination> Map<TDestination>(IQueryable source)
        {
            return source.ProjectTo<TDestination>();
        }
    }
}