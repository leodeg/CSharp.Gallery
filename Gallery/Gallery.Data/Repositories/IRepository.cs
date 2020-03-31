using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public interface IRepository<T> : IDisposable where T : class
	{
		T Get(int id);
		IEnumerable<T> Get();
		void Create(T entity);
		void Update(int id, T entity);
		bool Delete(int id);
		Task<bool> SaveAsync();
	}
}
