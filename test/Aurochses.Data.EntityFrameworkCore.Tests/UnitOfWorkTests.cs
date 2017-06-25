using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Aurochses.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using Moq;
using Xunit;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void Inherit_IUnitOfWork()
        {
            // Arrange
            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);

            // Act
            var unitOfWork = new UnitOfWork(mockDbContext.Object);

            // Assert
            Assert.IsAssignableFrom<IUnitOfWork>(unitOfWork);
        }

        [Fact]
        public void Dispose_UnitOfWork_Success()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Dispose_UnitOfWork_Success));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");

            UnitOfWork unitOfWork;

            using(unitOfWork = new UnitOfWork(fakeDbContext))
            {
                unitOfWork.Commit();
            }

            // Act
            unitOfWork.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => unitOfWork.Commit());
        }

        [Fact]
        public void Dispose_UnitOfWorkWithNullDbContext_Success()
        {
            // Arrange
            FakeUnitOfWork unitOfWork;

            using (unitOfWork = new FakeUnitOfWork())
            {
                var repository = unitOfWork.EntityRepository;

                if (repository == null) throw new ArgumentNullException(nameof(repository));
            }

            // Act
            unitOfWork.Dispose();

            // Assert
            try
            {
                var repository = unitOfWork.EntityRepository;

                if (repository == null) throw new ArgumentNullException(nameof(repository));
            }
            catch(Exception ex)
            {
                Assert.IsType<ObjectDisposedException>(ex);
            }
        }

        [Fact]
        public void Dispose_FalseDispose_Success()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Dispose_FalseDispose_Success));
            var unitOfWork = new FakeUnitOfWork(
                dbContext => new Repository<Entity<int>, int>(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            // Act
            unitOfWork.DoNotDispose();

            // Assert
            Assert.NotNull(unitOfWork.EntityRepository);
        }

        [Fact]
        public void Commit_InsertNewEntity_AffectedOneRow()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Commit_InsertNewEntity_AffectedOneRow));
            var unitOfWork = new FakeUnitOfWork(
                dbContext => new Repository<Entity<int>, int>(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            var entity = new Entity<int>();

            unitOfWork.EntityRepository.Insert(entity);

            // Act & Assert
            Assert.Equal(1, unitOfWork.Commit());
        }

        [Fact]
        public void Commit_UdateNonexistentEntity_DataStorageException()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Commit_UdateNonexistentEntity_DataStorageException));
            var unitOfWork = new FakeUnitOfWork(
                dbContext => new Repository<Entity<int>, int>(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            var entity = new Entity<int> { Id = -1 };

            unitOfWork.EntityRepository.Update(entity);

            // Act & Assert
            Assert.Throws<DataStorageException>(() => unitOfWork.Commit());
        }
    }
}