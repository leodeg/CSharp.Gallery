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
	public class ImageController : Controller
	{
		private readonly IImageRepository imageRepository;
		private readonly ITagRepository tagRepository;

		private readonly string ImageForm = "ImageForm";

		public ImageController(IImageRepository imageRepository, ITagRepository tagRepository)
		{
			this.imageRepository = imageRepository;
			this.tagRepository = tagRepository;
		}

		public IActionResult Index(string tag = "")
		{
			var model = string.IsNullOrEmpty(tag) ? imageRepository.Get() : imageRepository.GetWithTag(tag);
			return View(model);
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
				return NotFound();

			var image = imageRepository.Get(id.Value);
			if (image == null)
				return NotFound();

			return View(image);
		}

		public IActionResult Create()
		{
			var model = new UploadImageModel();
			return View(ImageForm, model);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var image = imageRepository.Get(id.Value);
			if (image == null)
				return NotFound();

			var uploadModel = new UploadImageModel()
			{
				Id = image.Id,
				Title = image.Title,
				Tags = image.Tags
			};

			return View(ImageForm, uploadModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Save(IFormFile file, UploadImageModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(ImageForm, model);
			}

			var image = new Image()
			{
				Id = model.Id,
				Title = model.Title,
				Tags = model.Tags
			};

			if (image == null)
				throw new ArgumentNullException();

			SaveTags(image);
			await SaveImage(file, image);

			await tagRepository.SaveAsync();
			await imageRepository.SaveAsync();
			return RedirectToAction(nameof(Index), nameof(Gallery));
		}

		private void SaveTags(Image image)
		{
			if (!string.IsNullOrEmpty(image.Tags))
				foreach (var tag in image.TagsList)
					tagRepository.Create(tag);
		}

		private async Task SaveImage(IFormFile file, Image image)
		{
			if (image.Id == 0)
				await imageRepository.Create(image, file);
			else await imageRepository.Update(image.Id, image, file);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			if (imageRepository.Delete(id.Value))
				await imageRepository.SaveAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}