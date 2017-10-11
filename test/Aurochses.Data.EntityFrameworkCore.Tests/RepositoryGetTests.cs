using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Xunit;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class RepositoryGetTests : IClassFixture<RepositoryFixture>
    {
        private readonly RepositoryFixture _fixture;

        public RepositoryGetTests(RepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Get_EntityExistsInRepository_Entity()
        {
            // Arrange & Act & Assert
            Assert.Equal(new FakeEntity { Id = 1 }, _fixture.UnitOfWork.FakeEntityRepository.Get(1));
        }

        [Fact]
        public void Get_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act & Assert
            Assert.Null(_fixture.UnitOfWork.FakeEntityRepository.Get(0));
        }

        [Fact]
        public void GetTModel_EntityExistsInRepository_Model()
        {
            // Arrange & Act & Assert
            Assert.Equal(new FakeModel { Id = 1 }, _fixture.UnitOfWork.FakeEntityRepository.Get<FakeModel>(_fixture.DataMapper, 1));
        }
    }
}