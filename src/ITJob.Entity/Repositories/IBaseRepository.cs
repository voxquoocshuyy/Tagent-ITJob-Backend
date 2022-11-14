using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ITJob.Entity.Repositories;

    /// <summary>
    /// Interface for Generic repository.
    /// </summary>
    /// <typeparam name="TEntity">entity class.</typeparam>
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets a table.
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Entry provide access to change tracking info and operations for entity.
        /// </summary>
        /// <param name="entity">entity object.</param>
        /// <returns>entity entry.</returns>
        EntityEntry<TEntity> EntityE(TEntity entity);

        /// <summary>
        /// Get all entities.
        /// </summary>
        /// <returns>Entities.</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Get entity by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="includeProperties">Lets the caller provide an array of properties for eager loading.</param>
        /// <returns>Entity.</returns>
        TEntity GetById(
            object id,
            Expression<Func<TEntity, object>>[] includeProperties = null);

        /// <summary>
        /// Get async entity by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="includeProperties">Lets the caller provide an array of properties for eager loading.</param>
        /// <returns>Entity.</returns>
        Task<TEntity> GetByIdAsync(
            object id,
            Expression<Func<TEntity, object>>[] includeProperties = null);

        /// <summary>
        /// Get entities with condition.
        /// </summary>
        /// <param name="filter">Condition Filters.</param>
        /// <param name="orderBy">Column to order the results by.</param>
        /// <param name="includeProperties">Lets the caller provide an array of properties for eager loading.</param>
        /// <returns>Entities.</returns>
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>>[] includeProperties = null);

        /// <summary>
        /// Get an entity with condition.
        /// </summary>
        /// <param name="filter">Condition Filters.</param>
        /// <param name="includeProperties">Lets the caller provide an array of properties for eager loading.</param>
        /// <returns>Entity.</returns>
        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includeProperties = null);

        /// <summary>
        /// Get async an entity with condition.
        /// </summary>
        /// <param name="filter">Condition Filters.</param>
        /// <param name="includeProperties">Lets the caller provide an array of properties for eager loading.</param>
        /// <returns>Entity.</returns>
        Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includeProperties = null);

        /// <summary>
        /// Insert entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert async entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>async operation.</returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Insert list of entity.
        /// </summary>
        /// <param name="entities">Entities.</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert async list of entity.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <returns>async operation.</returns>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        void Delete(IEnumerable<TEntity> entities);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

    }