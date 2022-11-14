using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ITJob.Entity.Repositories;

/// <summary>
    /// Generic repository.
    /// </summary>
    /// <typeparam name="TEntity">entity class.</typeparam>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbset;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="context">Object context.</param>
        public BaseRepository(DbContext context)
        {
            this.CurrentContext = context;
            this._dbset = this.CurrentContext.Set<TEntity>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="context">Object context.</param>
        /// <param name="dbsetExist">dbSet.</param>
        public BaseRepository(DbContext context, DbSet<TEntity> dbsetExist)
        {
            this.CurrentContext = context;
            this._dbset = dbsetExist;
        }

        /// <summary>
        /// Gets or sets current context.
        /// </summary>
        public DbContext CurrentContext { get; set; }

        /// <inheritdoc/>
        public IQueryable<TEntity> Table => this._dbset;

        /// <inheritdoc/>
        public EntityEntry<TEntity> EntityE(TEntity entity) => this.CurrentContext.Entry<TEntity>(entity);

        /// <inheritdoc/>
        public virtual IQueryable<TEntity> GetAll()
        {
            return this._dbset.AsQueryable();
        }

        /// <inheritdoc/>
        public virtual TEntity GetById(object id, Expression<Func<TEntity, object>>[] includeProperties = null)
        {
            DbSet<TEntity> query = this._dbset;

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => (DbSet<TEntity>)current.Include(include));
            }

            return query.Find(id);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetByIdAsync(object id, Expression<Func<TEntity, object>>[] includeProperties = null)
        {
            DbSet<TEntity> query = this._dbset;

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => (DbSet<TEntity>)current.Include(include));
            }

            return await query.FindAsync(id);
        }

        /// <inheritdoc/>
        public virtual IQueryable<TEntity> Get(
             Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Expression<Func<TEntity, object>>[] includeProperties = null)
        {
            IQueryable<TEntity> query = this._dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }
            else
            {
                return query;
            }
        }

        /// <inheritdoc/>
        public virtual TEntity GetFirstOrDefault(
             Expression<Func<TEntity, bool>> filter = null,
             Expression<Func<TEntity, object>>[] includeProperties = null)
        {
            IQueryable<TEntity> query = this._dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            return query.FirstOrDefault();
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(
             Expression<Func<TEntity, bool>> filter = null,
             Expression<Func<TEntity, object>>[] includeProperties = null)
        {
            IQueryable<TEntity> query = this._dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }
            var a = await query.FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this._dbset.Add(entity);
            
        }

        /// <inheritdoc/>
        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await this._dbset.AddAsync(entity);
            
        }

        /// <inheritdoc/>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            this._dbset.AddRange(entities);

        }

        /// <inheritdoc/>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await this._dbset.AddRangeAsync(entities);
            
        }

        /// <inheritdoc/>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this._dbset.Attach(entity);
            this.CurrentContext.Update<TEntity>(entity);
            
        }

        /// <inheritdoc/>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var enumerable = entities as TEntity[] ?? entities.ToArray();
            this._dbset.AttachRange(enumerable);
            this.CurrentContext.UpdateRange(enumerable);
            
        }

        /// <inheritdoc/>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (this.CurrentContext.Entry(entity).State == EntityState.Detached)
            {
                this._dbset.Attach(entity);
            }

            this._dbset.Remove(entity);
        }

        /// <inheritdoc/>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                if (this.CurrentContext.Entry(entity).State == EntityState.Detached)
                {
                    this._dbset.Attach(entity);
                }
            }

            this._dbset.RemoveRange(entities);
            
        }
        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CurrentContext.SaveChangesAsync(cancellationToken);
        }

    }