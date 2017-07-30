using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Aurochses.Data.EntityFrameworkCore.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Aurochses.Data.EntityFrameworkCore.Tests
{
    public class RepositoryTests : IClassFixture<RepositoryFixture>
    {
        private readonly RepositoryFixture _fixture;

        public RepositoryTests(RepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Inherit_IRepository()
        {
            // Arrange
            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);

            // Act
            var repository = new Repository<Entity<int>, int>(mockDbContext.Object);

            // Assert
            Assert.IsAssignableFrom<IRepository<Entity<int>, int>>(repository);
        }

        [Fact]
        public void Get_EntityExistsInRepository_EqualById()
        {
            // Arrange
            const int id = 1;

            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            mockDbContext
                .Setup(m => m.Set<Entity<int>>())
                .Returns(
                    EntityFrameworkMockHelpers.MockDbSet(
                        new List<Entity<int>>
                        {
                            new Entity<int> { Id = id }
                        }
                    )
                );

            var repository = new Repository<Entity<int>, int>(mockDbContext.Object);

            // Act & Assert
            Assert.Equal(id, repository.Get(id).Id);
        }

        [Fact]
        public void Get_EntityNotExistsInRepository_Null()
        {
            // Arrange
            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            mockDbContext
                .Setup(m => m.Set<Entity<int>>())
                .Returns(
                    EntityFrameworkMockHelpers.MockDbSet(
                        new List<Entity<int>>
                        {
                            new Entity<int> { Id = 1 }
                        }
                    )
                );

            var repository = new Repository<Entity<int>, int>(mockDbContext.Object);

            // Act & Assert
            Assert.Null(repository.Get(2));
        }

        [Fact]
        public void GetTModel_EntityExistsInRepository_EqualById()
        {
            // Arrange
            const int id = 1;

            // Act
            var model = _fixture.UnitOfWork.EntityRepository.Get<FakeModel>(_fixture.Mapper, id);

            // Assert
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public void GetTModel_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act
            var model = _fixture.UnitOfWork.EntityRepository.Get<FakeModel>(_fixture.Mapper, 0);

            // Assert
            Assert.Null(model);
        }

        [Fact]
        public void Find_Filter_Success()
        {
            // Arrange
            const int id = 1;

            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            mockDbContext
                .Setup(m => m.Set<Entity<int>>())
                .Returns(
                    EntityFrameworkMockHelpers.MockDbSet(
                        new List<Entity<int>>
                        {
                            new Entity<int> { Id = id }
                        }
                    )
                );

            var repository = new Repository<Entity<int>, int>(mockDbContext.Object);

            // Act
            var list = repository.Find(x => x.Id == id);

            // Assert
            Assert.Single(list);
            Assert.Equal(id, list[0].Id);
        }

        [Fact]
        public void FindTModel_Filter_Success()
        {
            // Arrange
            const int id = 1;

            // Act
            var list = _fixture.UnitOfWork.EntityRepository.Find<FakeModel>(_fixture.Mapper, x => x.Id == id);

            // Assert
            Assert.Single(list);
            Assert.Equal(id, list[0].Id);
        }

        [Fact]
        public void Exists_EntityExistsInRepository_True()
        {
            // Arrange
            const int id = 1;

            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            mockDbContext
                .Setup(m => m.Set<Entity<int>>())
                .Returns(
                    EntityFrameworkMockHelpers.MockDbSet(
                        new List<Entity<int>>
                        {
                            new Entity<int> { Id = id }
                        }
                    )
                );

            var repository = new Repository<Entity<int>, int>(mockDbContext.Object);

            // Act & Assert
            Assert.True(repository.Exists(id));
        }

        [Fact]
        public void Exists_EntityNotExistsInRepository_False()
        {
            // Arrange
            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            mockDbContext
                .Setup(m => m.Set<Entity<int>>())
                .Returns(
                    EntityFrameworkMockHelpers.MockDbSet(
                        new List<Entity<int>>
                        {
                            new Entity<int> { Id = 1 }
                        }
                    )
                );

            var repository = new Repository<Entity<int>, int>(mockDbContext.Object);

            // Act & Assert
            Assert.False(repository.Exists(2));
        }

        [Fact]
        public void Exists_Filter_Success()
        {
            // Arrange
            const int id = 1;

            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            mockDbContext
                .Setup(m => m.Set<Entity<int>>())
                .Returns(
                    EntityFrameworkMockHelpers.MockDbSet(
                        new List<Entity<int>>
                        {
                            new Entity<int> { Id = id }
                        }
                    )
                );

            var repository = new Repository<Entity<int>, int>(mockDbContext.Object);

            // Act & Assert
            Assert.True(repository.Exists(x => x.Id == id));
        }

        [Fact]
        public void Insert_NewEntity_Inserted()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Insert_NewEntity_Inserted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var repository = new Repository<Entity<int>, int>(fakeDbContext);

            var entity = new Entity<int>();

            // Act
            var insertedEntity = repository.Insert(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(entity.Id, insertedEntity.Id);
        }

        [Fact]
        public void Update_ExistingEntity_Updated()
        {
            // Arrange
            const int id = 10;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Update_ExistingEntity_Updated));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var entity = fakeDbContext.Add(new Entity<int> { Id = id }).Entity;
            fakeDbContext.SaveChanges();

            var repository = new Repository<Entity<int>, int>(fakeDbContext);

            // Act
            entity = repository.Update(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(id, entity.Id);
        }

        [Fact]
        public void Delete_ExistingEntity_Deleted()
        {
            // Arrange
            const int id = 13;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Delete_ExistingEntity_Deleted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var entity = fakeDbContext.Add(new Entity<int> { Id = id }).Entity;
            fakeDbContext.SaveChanges();

            fakeDbContext.Entry(entity).State = EntityState.Detached;

            var repository = new Repository<Entity<int>, int>(fakeDbContext);

            // Act
            repository.Delete(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(0, fakeDbContext.Set<Entity<int>>().Count());
        }

        [Fact]
        public void Delete_ExistingEntity_DeletedById()
        {
            // Arrange
            const int id = 15;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Delete_ExistingEntity_DeletedById));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            fakeDbContext.Add(new Entity<int> { Id = id });
            fakeDbContext.SaveChanges();

            var repository = new Repository<Entity<int>, int>(fakeDbContext);

            // Act
            repository.Delete(id);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(0, fakeDbContext.Set<Entity<int>>().Count());
        }
    }
}