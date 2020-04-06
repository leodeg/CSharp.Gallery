using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Models;
using Gallery.Data.Repositories;
using Gallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Controllers
{

	public class GalleryAlbumsController : Controller
	{
		private readonly AlbumsImagesRepository albumsImagesRepository;
		private readonly AlbumsRepository albumsRepository;
		private readonly ImageRepository imageRepository;

		private readonly int ItemsPerPage = 40;

		public GalleryAlbumsController(AlbumsImagesRepository albumsImagesRepository, AlbumsRepository albumsRepository, ImageRepository imageRepository)
		{
			this.albumsImagesRepository = albumsImagesRepository;
			this.albumsRepository = albumsRepository;
			this.imageRepository = imageRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Album(int? albumId, int page = 1)
		{
			if (albumId == null)
				return NotFound();

			IEnumerable<Image> images = albumsImagesRepository.GetImagesFromAlbum(albumId.Value);

			int totalImages = images.Count();
			if (totalImages > ItemsPerPage)
				images = images.Skip((page - 1) * ItemsPerPage).Take(ItemsPerPage);

			var model = new GalleryAlbumViewModel()
			{
				Images = images,
				AlbumTitle = albumsRepository.GetAlbumTitle(albumId.Value),
				PagingInformation = new PagingInformation()
				{
					CurrentPage = page,
					ItemsPerPage = ItemsPerPage,
					TotalItems = totalImages
				}
			};

			return View(model);
		}

		public IActionResult ImageDetails(int? id)
		{
			if (id == null)
				return NotFound();

			var image = imageRepository.Get(id.Value);
			if (image == null)
				return NotFound();

			return RedirectToAction(nameof(ImageDetails), nameof(Gallery), image);
		}
	}
}