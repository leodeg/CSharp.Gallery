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

			var image = imageRepository.Get(id.Value);
			if (image == null)
				return NotFound();

			var uploadModel = new UploadImageModel()
			{
				Id = image.Id,
				Title = image.Title,
				Tags = image.Tags,
				Url = image.Url,
				AllAlbums = albumsRepository.Get(),
				Albums = image.AlbumImages.Select(a => a.Album.Title).ToList()
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

			if (image == null)
				throw new ArgumentNullException();

			SaveTags(image);
			await SaveImage(file, image, albums);
			await imageRepository.SaveChangesAsync();

			return RedirectToAction(nameof(Index), nameof(Gallery));
		}

		private void SaveTags(Image image)
		{
			if (!string.IsNullOrEmpty(image.Tags))
				foreach (var tag in image.TagsList)
					tagRepository.Create(tag);
		}

		private async Task SaveImage(IFormFile file, Image image, List<int> albums)
		{
			if (image != null && albums != null)
			{
				var imageAlbums = albumsImagesRepository.Get().Where(i => i.ImageId == image.Id);
				var allAlbums = albumsRepository.Get();

				var addedAlbums = albums.Except(imageAlbums.Select(i => i.AlbumId));
				var removedAlbums = imageAlbums.Select(i => i.AlbumId).Except(albums);

				imageRepository.AddAlbums(image, addedAlbums);
				imageRepository.RemoveAlbums(image, removedAlbums);
			}

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
			if (!string.IsNullOrEmpty(image.Url))
				fileManager.DeleteImage(image.Url);

			imageRepository.Remove(imageRepository.Get(id.Value));
			await imageRepository.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}