using Gallery.Data.Managers;
using Gallery.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class ImageRepository : IImageRepository
	{
		private GalleryDataDbContext context;
		private readonly IFileManager fileManager;

		public ImageRepository(GalleryDataDbContext context, IFileManager fileManager)
		{
			this.context = context;
			this.fileManager = fileManager;
		}

		public IEnumerable<Image> Get()
		{
			return context.Images.OrderBy(x => x.Title);
		}

		public IEnumerable<Image> GetWithTag(string tag)
		{
			return context.Images.Where(c => c.Tags.Contains(tag)).OrderBy(x => x.Title);
		}

		public Image Get(int id)
		{
			return context.Images.FirstOrDefault(x => x.Id == id);
		}

		public Image Get(string title)
		{
			return context.Images.SingleOrDefault(x => x.Title == title);
		}

		public void Create(Image entity)
		{
			entity.Created = DateTime.Now;
			context.Images.Add(entity);
		}

		public void Create(string title)
		{
			context.Images.Add(new Image() { Title = title.Trim(), Created = DateTime.Now });
		}
		public async Task Create(Image entity, IFormFile file)
		{
			if (file != null)
				entity.Url = await fileManager.SaveImage(file);
			context.Images.Add(entity);
		}


		public async void Update(int id, Image entity)
		{
			await Update(id, entity, null);
		}

		public async Task Update(int id, Image entity, IFormFile file)
		{
			if (entity == null)
				throw new ArgumentNullException();

			Image oldImage = context.Images.FirstOrDefault(x => x.Id == id);
			if (entity == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldImage.Title = entity.Title;

			if (entity.Tags != null)
				oldImage.Tags = entity.Tags;

			if (file != null)
				oldImage.Url = await fileManager.SaveOrCreateImage(oldImage.Url, entity.Url, file);
		}

		public bool Delete(int id)
		{
			Image image = context.Images.FirstOrDefault(x => x.Id == id);
			if (image != null)
			{
				context.Images.Remove(image);
				return true;
			}
			return false;
		}

		public bool Delete(string title)
		{
			Image image = this.Get(title);
			if (image != null)
			{
				if (!string.IsNullOrEmpty(image.Url))
					fileManager.DeleteImage(image.Url);

				context.Images.Remove(image);
				return true;
			}
			return false;
		}

		public async Task<bool> SaveAsync()
		{
			if (await context.SaveChangesAsync() > 0)
				return true;
			return false;
		}

		private bool disposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
