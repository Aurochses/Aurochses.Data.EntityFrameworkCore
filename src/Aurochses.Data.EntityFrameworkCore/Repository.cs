using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Aurochses.Data.EntityFrameworkCore
{
    /// <summary>
    /// Repository for data layer
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TType">The type of the T type.</typeparam>
    /// <seealso>
    ///     <cref>Aurochses.Data.IRepository{TEntity, TType}</cref>
    /// </seealso>
    public class Repository<TEntity, TType> : IRepository<TEntity, TType>
        where TEntity : class, IEntity<TType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity, TType}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>The database context.</value>
        protected DbContext DbContext { get; }

        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <value>The database set.</value>
        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        private IQueryable<TEntity> Where(TType id)
        {
            return DbSet.Where(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Gets entity of type T from repository by identifier.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual TEntity Get(TType id)
        {
            return Where(id).FirstOrDefault();
        }

        /// <summary>
        /// Gets model of type T from repository by identifier.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><cref>TModel</cref></returns>
        public virtual TModel Get<TModel>(IDataMapper dataMapper, TType id)
        {
            return dataMapper.Map<TModel>(Where(id)).FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously gets entity of type T from repository by identifier.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual async Task<TEntity> GetAsync(TType id)
        {
            return await Where(id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Asynchronously gets model of type T from repository by identifier.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><cref>Task{TModel}</cref>.</returns>
        public virtual async Task<TModel> GetAsync<TModel>(IDataMapper dataMapper, TType id)
        {
            return await dataMapper.Map<TModel>(Where(id)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TEntity}</cref>.</returns>
        public IList<TEntity> GetList(QueryParameters<TEntity, TType> queryParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TModel}</cref>.</returns>
        public IList<TModel> GetList<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously gets entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TEntity}</cref>.</returns>
        public Task<IList<TEntity>> GetListAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously gets models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{IList{TModel}}</cref>.</returns>
        public Task<IList<TModel>> GetListAsync<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets paged list of entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>PagedResult{TEntity}</cref>.</returns>
        public PagedResult<TEntity> GetPagedList(QueryParameters<TEntity, TType> queryParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>PagedResult{TModel}</cref>.</returns>
        public PagedResult<TModel> GetPagedList<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously gets paged list of entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{PagedResult{TEntity}}</cref>.</returns>
        public Task<PagedResult<TEntity>> GetPagedListAsync(QueryParameters<TEntity, TType> queryParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="dataMapper">The data mapper.</param>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{PagedResult{TModel}}</cref>.</returns>
        public Task<PagedResult<TModel>> GetPagedListAsync<TModel>(IDataMapper dataMapper, QueryParameters<TEntity, TType> queryParameters)
        {
            throw new NotImplementedException();
        }

        private IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        /// <summary>
        /// Checks if entity of type T with identifier exists in repository.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual bool Exists(TType id)
        {
            return Where(id).Any();
        }

        /// <summary>
        /// Asynchronously checks if entity of type T with identifier exists in repository.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual async Task<bool> ExistsAsync(TType id)
        {
            return await Where(id).AnyAsync();
        }

        /// <summary>
        /// Checks if any entity of type T satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public bool Exists(QueryParameters<TEntity, TType> queryParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously checks if any entity of type T satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public Task<bool> ExistsAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a number that represents how many entities in repository satisfy a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns>A number that represents how many entities in repository satisfy a query parameters.</returns>
        public int Count(QueryParameters<TEntity, TType> queryParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously returns a number that represents how many entities in repository satisfy a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns>A number that represents how many entities in repository satisfy a query parameters.</returns>
        public Task<int> CountAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Marks entity as modified.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected void MarkAsModified(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Inserts entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Insert(TEntity entity)
        {
            return DbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Asynchronously inserts entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var entityEntry = await DbSet.AddAsync(entity);

            return entityEntry.Entity;
        }

        /// <summary>
        /// Updates entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="startTrackProperties">if set to <c>true</c> marks entity as modified.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Update(TEntity entity, bool startTrackProperties = false)
        {
            DbSet.Attach(entity);

            if (!startTrackProperties)
            {
                MarkAsModified(entity);
            }

            return entity;
        }

        /// <summary>
        /// Determines whether the specified entity is detached.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if the specified entity is detached; otherwise, <c>false</c>.</returns>
        protected bool IsDetached(TEntity entity)
        {
            return DbContext.Entry(entity).State == EntityState.Detached;
        }

        /// <summary>
        /// Marks entity as deleted.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected void MarkAsDeleted(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// Deletes the specified entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void Delete(TType id)
        {
            Delete(Get(id));
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(TEntity entity)
        {
            if (IsDetached(entity))
            {
                DbSet.Attach(entity);
            }

            MarkAsDeleted(entity);

            DbSet.Remove(entity);
        }
    }
}