using System;
using System.Threading.Tasks;
using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Aurochses.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class UnitOfWorkAsyncTests : IDisposable
    {
        private readonly FakeUnitOfWork _unitOfWork;

        public UnitOfWorkAsyncTests()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(UnitOfWorkAsyncTests));

            _unitOfWork = new FakeUnitOfWork(
                dbContext => new Repository<Entity<int>, int>(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        [Fact]
        public async Task CommitAsync_InsertNewEntity_AffectedOneRow()
        {
            // Arrange
            var entity = new Entity<int>();

            await _unitOfWork.EntityRepository.InsertAsync(entity);

            // Act & Assert
            Assert.Equal(1, await _unitOfWork.CommitAsync());
        }

        [Fact]
        public async Task CommitAsync_UdateNonexistentEntity_DataStorageException()
        {
            // Arrange
            var entity = new Entity<int> { Id = -1 };

            _unitOfWork.EntityRepository.Update(entity);

            // Act & Assert
            await Assert.ThrowsAsync<DataStorageException>(async () => await _unitOfWork.CommitAsync());
        }
    }
}