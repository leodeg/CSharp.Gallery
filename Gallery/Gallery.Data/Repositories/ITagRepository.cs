using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public interface ITagRepository : IRepository<Tag>
	{
		void Create(string title);
		Tag Get(string title);
		bool Delete(string title);
	}
}
