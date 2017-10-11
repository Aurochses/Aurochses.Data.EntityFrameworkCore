using System.Threading.Tasks;
using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Xunit;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class RepositoryAsyncTests : IClassFixture<RepositoryFixture>
    {
        private readonly RepositoryFixture _fixture;

        public RepositoryAsyncTests(RepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_EntityExistsInRepository_EqualById()
        {
            // Arrange
            const int id = 1;

            // Act
            var entity = await _fixture.UnitOfWork.FakeEntityRepository.GetAsync(id);

            // Assert
            Assert.Equal(id, entity.Id);
        }

        [Fact]
        public async Task GetAsync_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act
            var entity = await _fixture.UnitOfWork.FakeEntityRepository.GetAsync(0);

            // Assert
            Assert.Null(entity);
        }

        [Fact]
        public async Task GetTModelAsync_EntityExistsInRepository_EqualById()
        {
            // Arrange
            const int id = 1;

            // Act
            var model = await _fixture.UnitOfWork.FakeEntityRepository.GetAsync<FakeModel>(_fixture.DataMapper, id);

            // Assert
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public async Task GetTModelAsync_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act
            var model = await _fixture.UnitOfWork.FakeEntityRepository.GetAsync<FakeModel>(_fixture.DataMapper, 0);

            // Assert
            Assert.Null(model);
        }

        [Fact]
        public async Task ExistsAsync_EntityExistsInRepository_True()
        {
            // Arrange & Act & Assert
            Assert.True(await _fixture.UnitOfWork.FakeEntityRepository.ExistsAsync(1));
        }

        [Fact]
        public async Task ExistsAsync_EntityNotExistsInRepository_False()
        {
            // Arrange & Act & Assert
            Assert.False(await _fixture.UnitOfWork.FakeEntityRepository.ExistsAsync(0));
        }

        [Fact]
        public async Task InsertAsync_NewEntity_Inserted()
        {
            // Arrange
            var entity = new FakeEntity();

            // Act
            var insertedEntity = await _fixture.UnitOfWork.FakeEntityRepository.InsertAsync(entity);
            await _fixture.UnitOfWork.CommitAsync();

            // Assert
            Assert.Equal(entity.Id, insertedEntity.Id);
        }
    }
}