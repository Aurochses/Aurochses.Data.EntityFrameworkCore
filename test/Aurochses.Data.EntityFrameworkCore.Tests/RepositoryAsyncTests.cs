using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurochses.Data.EntityFrameworkCore.Tests.Fakes;
using Aurochses.Data.Query;
using Aurochses.Xunit;
using Microsoft.EntityFrameworkCore;
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

        private static DbSet<FakeEntity> DbSet => new FakeDbContext(new DbContextOptionsBuilder<FakeDbContext>().UseInMemoryDatabase($"{nameof(RepositoryTests)}{Guid.NewGuid():N}").Options, "dbo").Set<FakeEntity>();

        public static IEnumerable<object[]> QueryParametersMemberData => new[]
        {
            new object[]
            {
                null,
                DbSet.AsQueryable(),
                DbSet.AsQueryable()
            },
            new object[]
            {
                new QueryParameters<FakeEntity, int>(),
                DbSet.AsQueryable(),
                DbSet.AsQueryable()
            },
            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Filter = new FilterRule<FakeEntity, int>(),
                    Sort = new SortRule<FakeEntity, int>(),
                    Page = new PageRule()
                },
                DbSet.AsQueryable(),
                DbSet.AsQueryable()
            },

            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Filter = new FilterRule<FakeEntity, int>
                    {
                        Expression = x => x.Id == 1
                    }
                },
                DbSet.AsQueryable().Where(x => x.Id == 1),
                DbSet.AsQueryable().Where(x => x.Id == 1)
            },

            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Sort = new SortRule<FakeEntity, int>
                    {
                        Expression = x => x.Id
                    }
                },
                DbSet.AsQueryable().OrderBy(x => (object) x.Id),
                DbSet.AsQueryable()
            },
            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Sort = new SortRule<FakeEntity, int>
                    {
                        SortOrder = SortOrder.Ascending,
                        Expression = x => x.Id
                    }
                },
                DbSet.AsQueryable().OrderBy(x => (object) x.Id),
                DbSet.AsQueryable()
            },
            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Sort = new SortRule<FakeEntity, int>
                    {
                        SortOrder = SortOrder.Descending,
                        Expression = x => x.Id
                    }
                },
                DbSet.AsQueryable().OrderByDescending(x => (object) x.Id),
                DbSet.AsQueryable()
            },

            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Page = new PageRule
                    {
                        Index = 1,
                        Size = 5
                    }
                },
                DbSet.AsQueryable().Skip(5 * 1).Take(5),
                DbSet.AsQueryable()
            },

            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Filter = new FilterRule<FakeEntity, int>
                    {
                        Expression = x => x.Id == 1
                    },
                    Sort = new SortRule<FakeEntity, int>
                    {
                        SortOrder = SortOrder.Descending,
                        Expression = x => x.Id
                    },
                    Page = new PageRule
                    {
                        Index = 1,
                        Size = 5
                    }
                },
                DbSet.AsQueryable().Where(x => x.Id == 1).OrderByDescending(x => (object) x.Id).Skip(5 * 1).Take(5),
                DbSet.AsQueryable().Where(x => x.Id == 1)
            }
        };

        private static void ValidateQueryParametersMemberData(IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            if (countQueryable == null) throw new ArgumentNullException(nameof(countQueryable));

            Assert.NotNull(queryable);
            Assert.NotNull(countQueryable);
        }

        #region Get

        [Fact]
        public async Task GetAsync_EntityExistsInRepository_Entity()
        {
            // Arrange & Act & Assert
            ObjectAssert.ValueEquals(_fixture.ExistingFakeEntity, await _fixture.UnitOfWork.FakeEntityRepository.GetAsync(_fixture.ExistingFakeEntity.Id));
        }

        [Fact]
        public async Task GetAsync_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act & Assert
            Assert.Null(await _fixture.UnitOfWork.FakeEntityRepository.GetAsync(0));
        }

        [Fact]
        public async Task GetTModelAsync_EntityExistsInRepository_Model()
        {
            // Arrange & Act & Assert
            ObjectAssert.ValueEquals(_fixture.ExistingFakeModel, await _fixture.UnitOfWork.FakeEntityRepository.GetAsync<FakeModel>(_fixture.DataMapper, _fixture.ExistingFakeModel.Id));
        }

        [Fact]
        public async Task GetTModelAsync_EntityNotExistsInRepository_Null()
        {
            // Arrange & Act & Assert
            Assert.Null(await _fixture.UnitOfWork.FakeEntityRepository.GetAsync<FakeModel>(_fixture.DataMapper, 0));
        }

        #endregion

        #region GetList

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task GetListAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).ToList(), await _fixture.UnitOfWork.FakeEntityRepository.GetListAsync(queryParameters));
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task GetListTModelAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            ObjectAssert.ValueEquals(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters)).ToList(), await _fixture.UnitOfWork.FakeEntityRepository.GetListAsync<FakeModel>(_fixture.DataMapper, queryParameters));
        }

        #endregion

        #region GetPagedList

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task GetPagedListAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            if (queryParameters == null || queryParameters.Page == null || queryParameters.Page.IsValid == false) return;

            // Arrange
            var pagedResultQuery = _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery(queryParameters);

            var expectedPagedResult = new PagedResult<FakeEntity>
            {
                PageIndex = queryParameters.Page.Index,
                PageSize = queryParameters.Page.Size,
                Items = pagedResultQuery.ToList(),
                TotalCount = _fixture.UnitOfWork.FakeEntityRepository.Count(queryParameters)
            };

            // Act & Assert
            ObjectAssert.ValueEquals(expectedPagedResult, await _fixture.UnitOfWork.FakeEntityRepository.GetPagedListAsync(queryParameters));
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task GetPagedListTModelAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            if (queryParameters == null || queryParameters.Page == null || queryParameters.Page.IsValid == false) return;

            // Arrange
            var pagedResultQuery = _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery<FakeModel>(_fixture.DataMapper, queryParameters);

            var expectedPagedResult = new PagedResult<FakeModel>
            {
                PageIndex = queryParameters.Page.Index,
                PageSize = queryParameters.Page.Size,
                Items = pagedResultQuery.ToList(),
                TotalCount = _fixture.UnitOfWork.FakeEntityRepository.Count(queryParameters)
            };

            // Act & Assert
            ObjectAssert.ValueEquals(expectedPagedResult, await _fixture.UnitOfWork.FakeEntityRepository.GetPagedListAsync<FakeModel>(_fixture.DataMapper, queryParameters));
        }

        #endregion

        #region Exists

        [Fact]
        public async Task ExistsAsync_EntityExistsInRepository_True()
        {
            // Arrange & Act & Assert
            Assert.True(await _fixture.UnitOfWork.FakeEntityRepository.ExistsAsync(_fixture.ExistingFakeEntity.Id));
        }

        [Fact]
        public async Task ExistsAsync_EntityNotExistsInRepository_False()
        {
            // Arrange & Act & Assert
            Assert.False(await _fixture.UnitOfWork.FakeEntityRepository.ExistsAsync(0));
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task ExistsQueryParametersAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).Any(), await _fixture.UnitOfWork.FakeEntityRepository.ExistsAsync(queryParameters));
        }

        #endregion

        #region Count

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task CountAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedCountQuery(queryParameters).Count(), await _fixture.UnitOfWork.FakeEntityRepository.CountAsync(queryParameters));
        }

        #endregion

        #region Insert

        [Fact]
        public async Task InsertAsync_NewEntity_Inserted()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(InsertAsync_NewEntity_Inserted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            var entity = new FakeEntity();

            // Act
            var insertedEntity = await repository.InsertAsync(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(entity.Id, insertedEntity.Id);
        }

        #endregion
    }
}