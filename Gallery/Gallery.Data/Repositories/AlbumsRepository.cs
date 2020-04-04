using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class AlbumsRepository : Repository<Album>
	{
		public AlbumsRepository(GalleryDataDbContext context) : base(context)
		{
		}

		public override void Update(int id, Album entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			var album = dbSet.FirstOrDefault(a => a.Id == id);
			if (album == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			album.Title = entity.Title;
		}
	}
}