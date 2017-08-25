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
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <typeparam name="TType">The type of the t type.</typeparam>
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
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Get(TType id)
        {
            return Where(id).FirstOrDefault();
        }

        /// <summary>
        /// Gets model of type T from repository by identifier.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>TModel.</returns>
        public virtual TModel Get<TModel>(IMapper mapper, TType id)
        {
            return mapper.Map<TModel>(Where(id)).FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously gets entity of type T from repository by identifier.
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
        /// <param name="mapper">The mapper.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;TModel&gt;.</returns>
        public virtual async Task<TModel> GetAsync<TModel>(IMapper mapper, TType id)
        {
            return await mapper.Map<TModel>(Where(id)).FirstOrDefaultAsync();
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
        /// Finds enities of type T from repository.
        /// </summary>
        /// <param name="filter">Query filter.</param>
        /// <returns><cref>IList{TEntity}</cref>.</returns>
        public virtual IList<TEntity> Find(Expression<Func<TEntity, bool>> filter = null)
        {
            return Query(filter).ToList();
        }

        /// <summary>
        /// Finds models of type T from repository.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="filter">Query filter.</param>
        /// <returns>IList&lt;TModel&gt;.</returns>
        public virtual IList<TModel> Find<TModel>(IMapper mapper, Expression<Func<TEntity, bool>> filter = null)
        {
            return mapper.Map<TModel>(Query(filter)).ToList();
        }

        /// <summary>
        /// Asynchronously finds enities of type T from repository.
        /// </summary>
        /// <param name="filter">Query filter.</param>
        /// <returns><cref>IList{TEntity}</cref>.</returns>
        public virtual async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await Query(filter).ToListAsync();
        }

        /// <summary>
        /// Asynchronously finds models of type T from repository.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Task&lt;IList&lt;TModel&gt;&gt;.</returns>
        public virtual async Task<IList<TModel>> FindAsync<TModel>(IMapper mapper, Expression<Func<TEntity, bool>> filter = null)
        {
            return await mapper.Map<TModel>(Query(filter)).ToListAsync();
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
        /// Checks if entity of type T exists in repository.
        /// </summary>
        /// <param name="filter">Query filter.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> filter = null)
        {
            return Query(filter).Any();
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
        /// Asynchronously checks if entity of type T exists in repository.
        /// </summary>
        /// <param name="filter">Query filter.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await Query(filter).AnyAsync();
        }

        /// <summary>
        /// Determines whether the specified entity is new.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if the specified entity is new; otherwise, <c>false</c>.</returns>
        protected bool IsNew(TEntity entity)
        {
            return entity.IsNew();
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

        /// <summary>
        /// Deletes the specified entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void Delete(TType id)
        {
            Delete(Get(id));
        }
    }
}