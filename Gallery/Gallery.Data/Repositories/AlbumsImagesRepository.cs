using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class AlbumsImagesRepository : Repository<AlbumImage>
	{
		public AlbumsImagesRepository(GalleryDataDbContext context) : base(context)
		{
		}

		#region Albums

		public IEnumerable<Album> GetAlbumsWithTitle(string title)
		{
			return context.AlbumImages.Where(album => album.Album.Title == title).Select(album => album.Album);
		}

		public IEnumerable<string> GetAlbumsTitlesWithImage(int imageId)
		{
			return context.AlbumImages.Where(album => album.ImageId == imageId).Select(album => album.Album.Title);
		}

		public IEnumerable<Album> GetAlbumsWithImage(int imageId)
		{
			return context.AlbumImages.Where(album => album.ImageId == imageId).Select(album => album.Album);
		}

		public IEnumerable<int> GetAlbumsIdsWithImage(int imageId)
		{
			return context.AlbumImages.Where(album => album.ImageId == imageId).Select(album => album.AlbumId);
		}

		#endregion

		#region Images

		public Image GetFirstImageFromAlbum(int albumId)
		{
			return context.AlbumImages.FirstOrDefault(ai => ai.AlbumId == albumId)?.Image;
		}

		public IEnumerable<Image> GetImagesFromAlbum(int albumId)
		{
			return context.AlbumImages.Where(ai => ai.AlbumId == albumId).Select(ai => ai.Image);
		}

		public void RemoveImageFromAllAlbums(int imageId)
		{
			var albums = context.AlbumImages.Where(album => album.ImageId == imageId);
			context.RemoveRange(albums);
		}

		public void AddImageToAlbum(Image image, Album album)
		{
			context.AlbumImages.Add(new AlbumImage { ImageId = image.Id, AlbumId = album.Id });
		}

		public void AddImageToAlbums(Image image, IEnumerable<Album> albums)
		{
			foreach (var album in albums)
			{
				context.AlbumImages.Add(new AlbumImage { ImageId = image.Id, AlbumId = album.Id });
			}
		}

		public void AddImageToAlbums(Image image, IEnumerable<int> albumsIds)
		{
			var albumsEntities = context.Albums.Where(a => albumsIds.Contains(a.Id));
			foreach (var album in albumsEntities)
			{
				context.AlbumImages.Add(new AlbumImage { ImageId = image.Id, AlbumId = album.Id });
			}
		}

		public bool RemoveImageFromAlbum(Image image, int id)
		{
			var album = context.AlbumImages.FirstOrDefault(a => a.ImageId == image.Id && a.AlbumId == id);
			if (album != null)
			{
				context.AlbumImages.Remove(album);
				return true;
			}
			return false;
		}

		public bool RemoveImageFromAlbums(Image image, IEnumerable<int> albums)
		{
			var album = context.AlbumImages.Where(a => a.ImageId == image.Id && albums.Contains(a.AlbumId));
			if (album != null)
			{
				context.AlbumImages.RemoveRange(album);
				return true;
			}
			return false;
		}

		#endregion
	}
}