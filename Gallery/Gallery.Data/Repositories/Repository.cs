using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		protected readonly GalleryDataDbContext context;
		protected readonly DbSet<TEntity> dbSet;
		private bool disposed = false;

		public Repository(GalleryDataDbContext context)
		{
			this.context = context;
			this.dbSet = context.Set<TEntity>();
		}

		public virtual void Create(TEntity entity)
		{
			dbSet.Add(entity);
		}

		public virtual TEntity Get(int id)
		{
			return dbSet.Find(id);
		}

		public virtual IEnumerable<TEntity> Get()
		{
			return dbSet.AsNoTracking().ToList();
		}

		public virtual IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
		{
			return dbSet.AsNoTracking().Where(predicate).ToList();
		}

		public virtual IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			return Include(includeProperties).ToList();
		}

		public virtual IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
			params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var query = Include(includeProperties);
			return query.Where(predicate).ToList();
		}

		private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> query = dbSet.AsNoTracking();
			return includeProperties
				.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
		}

		public virtual void Update(int id, TEntity entity)
		{
			context.Entry(entity).State = EntityState.Modified;
		}

		public void Remove(int id)
		{
			var entity = dbSet.Find(id);
			if (entity != null)
				dbSet.Remove(entity);
		}

		public virtual void Remove(TEntity entity)
		{
			dbSet.Remove(entity);
		}

		public virtual bool SaveChanges()
		{
			if (context.SaveChanges() > 0)
				return true;
			return false;
		}

		public virtual async Task<bool> SaveChangesAsync()
		{
			if (await context.SaveChangesAsync() > 0)
				return true;
			return false;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
