using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Repositories;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Controllers
{
	public class GalleryController : Controller
	{
		private readonly IImageRepository imageRepository;
		private readonly ITagRepository tagRepository;

		public GalleryController(IImageRepository imageRepository, ITagRepository tagRepository)
		{
			this.imageRepository = imageRepository;
			this.tagRepository = tagRepository;
		}

		public IActionResult Index()
		{
			var model = new GalleryIndexViewModel()
			{
				Images = imageRepository.Get()
			};

			return View(model);
		}
	}
}