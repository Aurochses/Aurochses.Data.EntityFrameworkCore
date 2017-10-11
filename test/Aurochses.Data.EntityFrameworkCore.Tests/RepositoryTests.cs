using System;
using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Aurochses.Data.Query;
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
            var model = _fixture.UnitOfWork.FakeEntityRepository.Get<FakeModel>(_fixture.DataMapper, id);

            // Assert
            Assert.Equal(id, model.Id);
        }

        [Fact]
        public void GetTModel_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act
            var model = _fixture.UnitOfWork.FakeEntityRepository.Get<FakeModel>(_fixture.DataMapper, 0);

            // Assert
            Assert.Null(model);
        }

        private static FakeRepository GetFakeRepository(string methodName)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(methodName);
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            return new FakeRepository(fakeDbContext);
        }

        private static void ValidateQuery<TType>(IQueryable<TType> expected, IQueryable<TType> actual)
        {
            if (expected == null) throw new ArgumentNullException(nameof(expected));
            if (actual == null) throw new ArgumentNullException(nameof(actual));

            Assert.Equal(expected.Expression.ToString(), actual.Expression.ToString());
        }

        [Fact]
        public void Query_QueryParametersIsNullOrEmpty_IQueryable()
        {
            // Arrange
            var repository = GetFakeRepository(nameof(Query_QueryParametersIsNullOrEmpty_IQueryable));

            // Act & Assert
            ValidateQuery(repository.ProtectedDbSet, repository.ProtectedQuery());
            ValidateQuery(repository.ProtectedDbSet, repository.ProtectedQuery(new QueryParameters<FakeEntity, int>()));
            ValidateQuery(repository.ProtectedDbSet, repository.ProtectedQuery(new QueryParameters<FakeEntity, int> { Filter = new FilterRule<FakeEntity, int>(), Sort = new SortRule<FakeEntity, int>(), Page = new PageRule() }));
        }

        [Fact]
        public void Query_QueryParametersWithFilter_IQueryableWithFilter()
        {
            // Arrange
            var repository = GetFakeRepository(nameof(Query_QueryParametersWithFilter_IQueryableWithFilter));

            Expression<Func<FakeEntity, bool>> queryParametersFilterExpression = x => x.Id == 1;
            var queryParameters = new QueryParameters<FakeEntity, int>
            {
                Filter = new FilterRule<FakeEntity, int>
                {
                    Expression = queryParametersFilterExpression
                }
            };

            // Act & Assert
            ValidateQuery(repository.ProtectedDbSet.Where(queryParametersFilterExpression), repository.ProtectedQuery(queryParameters));
        }

        [Fact]
        public void Query_QueryParametersWithSort_IQueryableWithOrderBy()
        {
            // Arrange
            var repository = GetFakeRepository(nameof(Query_QueryParametersWithSort_IQueryableWithOrderBy));

            Expression<Func<FakeEntity, object>> queryParametersSortExpression = x => x.Id == 1;
            var queryParameters = new QueryParameters<FakeEntity, int>
            {
                Sort = new SortRule<FakeEntity, int>
                {
                    Expression = queryParametersSortExpression
                }
            };

            // Act & Assert
            ValidateQuery(repository.ProtectedDbSet.OrderBy(queryParametersSortExpression), repository.ProtectedQuery(queryParameters));
        }

        [Fact]
        public void Query_QueryParametersWithSortAscending_IQueryableWithOrderBy()
        {
            // Arrange
            var repository = GetFakeRepository(nameof(Query_QueryParametersWithSortAscending_IQueryableWithOrderBy));

            Expression<Func<FakeEntity, object>> queryParametersSortExpression = x => x.Id == 1;
            var queryParameters = new QueryParameters<FakeEntity, int>
            {
                Sort = new SortRule<FakeEntity, int>
                {
                    SortOrder = SortOrder.Ascending,
                    Expression = queryParametersSortExpression
                }
            };

            // Act & Assert
            ValidateQuery(repository.ProtectedDbSet.OrderBy(queryParametersSortExpression), repository.ProtectedQuery(queryParameters));
        }

        [Fact]
        public void Query_QueryParametersWithSortDescending_IQueryableWithOrderBy()
        {
            // Arrange
            var repository = GetFakeRepository(nameof(Query_QueryParametersWithSortDescending_IQueryableWithOrderBy));

            Expression<Func<FakeEntity, object>> queryParametersSortExpression = x => x.Id == 1;
            var queryParameters = new QueryParameters<FakeEntity, int>
            {
                Sort = new SortRule<FakeEntity, int>
                {
                    SortOrder = SortOrder.Descending,
                    Expression = queryParametersSortExpression
                }
            };

            // Act & Assert
            ValidateQuery(repository.ProtectedDbSet.OrderByDescending(queryParametersSortExpression), repository.ProtectedQuery(queryParameters));
        }

        [Fact]
        public void Query_QueryParametersWithPage_IQueryableWithSkipTake()
        {
            // Arrange
            var repository = GetFakeRepository(nameof(Query_QueryParametersWithPage_IQueryableWithSkipTake));

            var queryParameters = new QueryParameters<FakeEntity, int>
            {
                Page = new PageRule
                {
                    Index = 1,
                    Size = 5
                }
            };

            // Act & Assert
            ValidateQuery(repository.ProtectedDbSet.Skip(queryParameters.Page.Size * queryParameters.Page.Index).Take(queryParameters.Page.Size), repository.ProtectedQuery(queryParameters));
        }

        [Fact]
        public void QueryTModel_Success()
        {
            // Arrange & Act & Assert
            ValidateQuery(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery()), _fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery<FakeModel>(_fixture.DataMapper));
            ValidateQuery(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(new QueryParameters<FakeEntity, int>())), _fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery<FakeModel>(_fixture.DataMapper, new QueryParameters<FakeEntity, int>()));
            ValidateQuery(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(new QueryParameters<FakeEntity, int> { Filter = new FilterRule<FakeEntity, int>(), Sort = new SortRule<FakeEntity, int>(), Page = new PageRule() })), _fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery<FakeModel>(_fixture.DataMapper, new QueryParameters<FakeEntity, int> { Filter = new FilterRule<FakeEntity, int>(), Sort = new SortRule<FakeEntity, int>(), Page = new PageRule() }));
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
        public void Insert_NewEntity_Inserted()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Insert_NewEntity_Inserted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            var entity = new FakeEntity();

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
            var entity = fakeDbContext.Add(new FakeEntity { Id = id }).Entity;
            fakeDbContext.SaveChanges();

            var repository = new Repository<FakeEntity, int>(fakeDbContext);

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
            var entity = fakeDbContext.Add(new FakeEntity { Id = id }).Entity;
            fakeDbContext.SaveChanges();

            fakeDbContext.Entry(entity).State = EntityState.Detached;

            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            // Act
            repository.Delete(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(0, fakeDbContext.Set<FakeEntity>().Count());
        }

        [Fact]
        public void Delete_ExistingEntity_DeletedById()
        {
            // Arrange
            const int id = 15;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Delete_ExistingEntity_DeletedById));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            fakeDbContext.Add(new FakeEntity { Id = id });
            fakeDbContext.SaveChanges();

            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            // Act
            repository.Delete(id);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(0, fakeDbContext.Set<FakeEntity>().Count());
        }
    }
}