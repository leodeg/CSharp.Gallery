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

		private readonly int ItemsPerPage = 40;

		public GalleryController(ImageRepository imageRepository, AlbumsImagesRepository albumsImagesRepository)
		{
			this.imageRepository = imageRepository;
			this.albumsImagesRepository = albumsImagesRepository;
		}

		public IActionResult Index(string tag = "", int page = 1)
		{
			IEnumerable<Image> images = string.IsNullOrEmpty(tag)
				? imageRepository.Get()
					: imageRepository.GetWithTag(tag);

			int totalImages = images.Count();
			if (totalImages > ItemsPerPage)
				images = images.Skip((page - 1) * ItemsPerPage).Take(ItemsPerPage);

			var model = new GalleryIndexViewModel()
			{
				Images = images,
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

			return View(image);
		}
	}
}