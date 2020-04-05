using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Repositories;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Components
{
	public class AlbumsCloud : ViewComponent
	{
		private readonly AlbumsRepository albumsRepository;

		public AlbumsCloud(AlbumsRepository albumsRepository)
		{
			this.albumsRepository = albumsRepository;
		}

		public IViewComponentResult Invoke()
		{
			var model = new AlbumsViewModel()
			{
				Albums = GetAlbums()
			};

			return View(model);
		}

		public IEnumerable<AlbumWithImage> GetAlbums()
		{
			var albums = albumsRepository.GetWithImages();
			List<AlbumWithImage> albumsWithImages = new List<AlbumWithImage>();

			foreach (var album in albums)
			{
				var albumWithImage = new AlbumWithImage()
				{
					Id = album.Id,
					Title = album.Title,
					ImageCount = album.AlbumImages.Count(),
					ImageUrl = album.AlbumImages.FirstOrDefault()?.Image.Url ?? ""
				};

				albumsWithImages.Add(albumWithImage);
			}

			return albumsWithImages;
		}
	}
}