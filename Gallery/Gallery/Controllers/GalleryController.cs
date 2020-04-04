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

		public GalleryController(ImageRepository imageRepository)
		{
			this.imageRepository = imageRepository;
		}

		public IActionResult Index(string tag = "")
		{
			var model = new GalleryIndexViewModel()
			{
				Images = string.IsNullOrEmpty(tag) ? imageRepository.Get() : imageRepository.GetWithTag(tag)
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