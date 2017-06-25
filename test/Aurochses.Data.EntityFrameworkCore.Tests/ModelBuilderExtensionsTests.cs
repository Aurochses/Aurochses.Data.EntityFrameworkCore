using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class ModelBuilderExtensionsTests
    {
        [Fact]
        public void AddConfiguration_Entity_Success()
        {
            // Arrange
            var modelBuilder = new ModelBuilder(new ConventionSet());
            var entityTypeConfiguration = new FakeEntityTypeConfiguration("dbo");

            // Act
            modelBuilder.AddConfiguration(entityTypeConfiguration);

            // Assert
            Assert.NotNull(modelBuilder.Model.FindEntityType(typeof (Entity<int>)));
        }
    }
}