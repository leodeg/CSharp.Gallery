using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Models;
using Gallery.Data.Repositories;
using Gallery.Models;
using Gallery.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Controllers
{
	public class ImageController : Controller
	{
		private readonly IImageService imageService;

		public ImageController(IImageService imageService)
		{
			this.imageService = imageService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Create()
		{
			return View(new Image());
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var image = imageService.GetById(id.Value);
			if (image == null)
				return NotFound();

			return View(image);
		}

		public IActionResult Save(Image image, IFormFile file)
		{
			if (!ModelState.IsValid)
			{
				return View(image);
			}

			imageService.Save(image);

			return View(image);
		}
	}
}