using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Repositories;
using Gallery.Models;
using Gallery.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Controllers
{
	public class GalleryController : Controller
	{
		private readonly IImageService imageService;

		public GalleryController(IImageService imageService)
		{
			this.imageService = imageService;
		}

		public IActionResult Index()
		{
			var model = new GalleryIndexViewModel()
			{
				Images = imageService.GetAll()
			};

			return View(model);
		}
	}
}