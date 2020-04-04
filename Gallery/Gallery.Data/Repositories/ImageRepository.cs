using Gallery.Data.Managers;
using Gallery.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class ImageRepository : Repository<Image>
	{
		public ImageRepository(GalleryDataDbContext context) : base(context)
		{
		}

		/// <summary>
		/// Include Album Images and Album Entities
		/// </summary>
		/// <param name="id">Image id</param>
		public override Image Get(int id)
		{
			return dbSet.Include(c => c.AlbumImages).ThenInclude(c => c.Album).FirstOrDefault(c => c.Id == id);
		}

		public override IEnumerable<Image> Get()
		{
			return dbSet.Include(c => c.AlbumImages);
		}

		public IEnumerable<Image> GetWithTag(string tag)
		{
			return dbSet.Include(c => c.AlbumImages).Where(c => c.Tags.Contains(tag)).OrderBy(x => x.Title);
		}

		public Image Get(string title)
		{
			return dbSet.SingleOrDefault(x => x.Title == title);
		}

		public void Create(string title)
		{
			dbSet.Add(new Image() { Title = title.Trim() });
		}

		public override void Update(int id, Image entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			Image oldImage = dbSet.FirstOrDefault(x => x.Id == id);
			if (entity == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldImage.Title = entity.Title;
			oldImage.Url = entity.Url;
			oldImage.Tags = entity.Tags;
		}

		public bool Remove(string title)
		{
			Image image = this.Get(title);
			if (image != null)
			{
				dbSet.Remove(image);
				return true;
			}
			return false;
		}

		// ------------------------------------
		// -------------- ALBUMS --------------
		// ------------------------------------

		public void AddAlbum(Image image, Album album)
		{
			context.AlbumImages.Add(new AlbumImage { ImageId = image.Id, AlbumId = album.Id });
		}

		public void AddAlbums(Image image, IEnumerable<Album> albums)
		{
			foreach (var album in albums)
			{
				context.AlbumImages.Add(new AlbumImage { ImageId = image.Id, AlbumId = album.Id });
			}
		}

		public void AddAlbums(Image image, IEnumerable<int> albums)
		{
			var albumsEntities = context.Albums.Where(a => albums.Contains(a.Id));
			foreach (var album in albumsEntities)
			{
				context.AlbumImages.Add(new AlbumImage { ImageId = image.Id, AlbumId = album.Id });
			}
		}

		public bool RemoveAlbum(Image image, int id)
		{
			var album = context.AlbumImages.FirstOrDefault(a => a.ImageId == image.Id && a.AlbumId == id);
			if (album != null)
			{
				context.AlbumImages.Remove(album);
				return true;
			}
			return false;
		}

		public bool RemoveAlbums(Image image, IEnumerable<int> albums)
		{
			var album = context.AlbumImages.Where(a => a.ImageId == image.Id && albums.Contains(a.AlbumId));
			if (album != null)
			{
				context.AlbumImages.RemoveRange(album);
				return true;
			}
			return false;
		}
	}
}
