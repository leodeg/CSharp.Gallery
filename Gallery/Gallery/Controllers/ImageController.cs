using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Managers;
using Gallery.Data.Models;
using Gallery.Data.Repositories;
using Gallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gallery.Controllers
{
	public class ImageController : Controller
	{
		private readonly ImageRepository imageRepository;
		private readonly TagRepository tagRepository;
		private readonly AlbumsRepository albumsRepository;
		private readonly AlbumsImagesRepository albumsImagesRepository;

		private readonly IFileManager fileManager;
		private readonly string ImageForm = "ImageForm";

		public ImageController(ImageRepository imageRepository, TagRepository tagRepository, AlbumsRepository albumsRepository, AlbumsImagesRepository albumsImagesRepository, IFileManager fileManager)
		{
			this.imageRepository = imageRepository;
			this.tagRepository = tagRepository;
			this.albumsRepository = albumsRepository;
			this.albumsImagesRepository = albumsImagesRepository;
			this.fileManager = fileManager;
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
			model.AllAlbums = albumsRepository.Get();
			model.Albums = new List<string>();
			return View(ImageForm, model);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var image = imageRepository.GetWithAlbums(id.Value);
			if (image == null)
				return NotFound();

			var uploadModel = new UploadImageModel()
			{
				Id = image.Id,
				Title = image.Title,
				Tags = image.Tags,
				Url = image.Url,
				AllAlbums = albumsRepository.Get(),
				Albums = albumsImagesRepository.GetAlbumsTitlesWithImage(image.Id)
			};

			return View(ImageForm, uploadModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Save(IFormFile file, UploadImageModel model, List<int> albums)
		{
			if (!ModelState.IsValid)
			{
				return View(ImageForm, model);
			}

			var image = new Image()
			{
				Id = model.Id,
				Title = model.Title,
				Tags = model.Tags,
				Url = model.Url
			};

			if (!string.IsNullOrEmpty(image.Tags))
				tagRepository.Create(image.TagsList);

			SaveAlbums(image, albums);
			await SaveImage(file, image);
			await imageRepository.SaveChangesAsync();

			return RedirectToAction(nameof(Index), nameof(Gallery));
		}

		private void SaveAlbums(Image image, List<int> albumsIds)
		{
			if (image != null && albumsIds != null)
			{
				IEnumerable<int> imageAlbumsIds = albumsImagesRepository.GetAlbumsIdsWithImage(image.Id);

				IEnumerable<int> addedAlbumsIds = albumsIds.Except(imageAlbumsIds);
				IEnumerable<int> removedAlbumsIds = imageAlbumsIds.Except(albumsIds);

				imageRepository.AddToAlbums(image, addedAlbumsIds);
				imageRepository.RemoveFromAlbums(image, removedAlbumsIds);
			}
		}

		private async Task SaveImage(IFormFile file, Image image)
		{
			if (image.Id == 0)
			{
				if (file != null)
					image.Url = await fileManager.SaveImage(file);
				imageRepository.Create(image);
			}
			else
			{
				if (file != null)
				{
					fileManager.DeleteImage(image.Url);
					image.Url = await fileManager.SaveImage(file);
				}
				imageRepository.Update(image.Id, image);
			}
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			var image = imageRepository.Get(id.Value);
			if (image == null)
				return NotFound();

			if (!string.IsNullOrEmpty(image.Url))
				fileManager.DeleteImage(image.Url);

			imageRepository.Remove(id.Value);
			await imageRepository.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}