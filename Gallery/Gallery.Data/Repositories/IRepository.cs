using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public interface IRepository<TEntity> : IDisposable where TEntity : class
	{
		TEntity Get(int id);
		IEnumerable<TEntity> Get();
		IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
		void Create(TEntity entity);
		void Update(int id, TEntity entity);
		void Remove(int id);
		void Remove(TEntity entity);
		bool SaveChanges();
		Task<bool> SaveChangesAsync();
	}
}