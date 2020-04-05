using Gallery.Data.Models;
using Microsoft.EntityFrameworkCore;
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

		public Album GetWithImages(int id)
		{
			return context.Albums.Include(album => album.AlbumImages).ThenInclude(albumImages => albumImages.Image).FirstOrDefault(album => album.Id == id);
		}

		public IEnumerable<Album> GetWithImages()
		{
			return context.Albums.Include(album => album.AlbumImages).ThenInclude(albumImages => albumImages.Image);
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