using System.Threading.Tasks;
using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Aurochses.Xunit;
using Xunit;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class RepositoryGetAsyncTests : IClassFixture<RepositoryFixture>
    {
        private readonly RepositoryFixture _fixture;

        public RepositoryGetAsyncTests(RepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_EntityExistsInRepository_Entity()
        {
            // Arrange & Act & Assert
            ObjectAssert.ValueEquals(new FakeEntity { Id = 1 }, await _fixture.UnitOfWork.FakeEntityRepository.GetAsync(1));
        }

        [Fact]
        public async Task GetAsync_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act & Assert
            Assert.Null(await _fixture.UnitOfWork.FakeEntityRepository.GetAsync(0));
        }

        [Fact]
        public async Task GetTModelAsync_EntityExistsInRepository_Entity()
        {
            // Arrange & Act & Assert
            ObjectAssert.ValueEquals(new FakeModel { Id = 1 }, await _fixture.UnitOfWork.FakeEntityRepository.GetAsync<FakeModel>(_fixture.DataMapper, 1));
        }

        [Fact]
        public async Task GetTModelAsync_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act & Assert
            Assert.Null(await _fixture.UnitOfWork.FakeEntityRepository.GetAsync<FakeModel>(_fixture.DataMapper, 0));
        }
    }
}