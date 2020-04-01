using Gallery.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public interface IImageRepository : IRepository<Image>
	{
		Image Get(string title);
		IEnumerable<Image> GetWithTag(string tag);
		bool Delete(string title);

		Task Create(Image entity, IFormFile file);
		Task Update(int id, Image entity, IFormFile file);
	}
}
