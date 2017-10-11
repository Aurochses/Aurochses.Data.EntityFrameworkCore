using Microsoft.EntityFrameworkCore;

namespace Aurochses.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeDbContext : DbContextBase
    {
        public FakeDbContext(DbContextOptions options, string schemaName)
            : base(options, schemaName)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FakeEntity>();
        }
    }
}