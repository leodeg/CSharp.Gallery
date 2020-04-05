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
		private readonly AlbumsImagesRepository albumsImagesRepository;

		public ImageRepository(GalleryDataDbContext context, AlbumsImagesRepository albumsImagesRepository) : base(context)
		{
			this.albumsImagesRepository = albumsImagesRepository;
		}

		public override Image Get(int id)
		{
			return dbSet.FirstOrDefault(c => c.Id == id);
		}

		/// <summary>
		/// Include Albums with current image.
		/// </summary>
		public Image GetWithAlbums(int id)
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

		public void AddToAlbum(Image image, Album album)
		{
			albumsImagesRepository.AddImageToAlbum(image, album);
		}

		public void AddToAlbums(Image image, IEnumerable<Album> albums)
		{
			albumsImagesRepository.AddImageToAlbums(image, albums);
		}

		public IEnumerable<string> GetAlbumsTitles (int imageId)
		{
			return albumsImagesRepository.GetAlbumsTitlesWithImage(imageId);
		}

		public void AddToAlbums(Image image, IEnumerable<int> albumsIds)
		{
			albumsImagesRepository.AddImageToAlbums(image, albumsIds);
		}

		public bool RemoveFromAlbum(Image image, int id)
		{
			return albumsImagesRepository.RemoveImageFromAlbum(image, id);
		}

		public bool RemoveFromAlbums(Image image, IEnumerable<int> albums)
		{
			return albumsImagesRepository.RemoveImageFromAlbums(image, albums);
		}

		public void RemoveFromAllAlbums(int imageId)
		{
			albumsImagesRepository.RemoveImageFromAllAlbums(imageId);
		}
	}
}
