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
	public class GalleryController : Controller
	{
		private readonly ImageRepository imageRepository;
		private readonly AlbumsImagesRepository albumsImagesRepository;

		public GalleryController(ImageRepository imageRepository, AlbumsImagesRepository albumsImagesRepository)
		{
			this.imageRepository = imageRepository;
			this.albumsImagesRepository = albumsImagesRepository;
		}

		public IActionResult Index(int? albumId, string tag = "")
		{
			var model = new GalleryIndexViewModel();

			if (albumId != null)
			{
				model.Images = albumsImagesRepository.GetImagesFromAlbum(albumId.Value);
			}
			else
			{
				model.Images = string.IsNullOrEmpty(tag) ? imageRepository.Get() : imageRepository.GetWithTag(tag);
			}

			return View(model);
		}

		public IActionResult ImageDetails(int? id)
		{
			if (id == null)
				return NotFound();

			var image = imageRepository.Get(id.Value);
			if (image == null)
				return NotFound();

			return View(image);
		}
	}
}