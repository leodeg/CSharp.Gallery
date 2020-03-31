using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class ImageRepository : IImageRepository
	{
		private GalleryDataDbContext context;

		public ImageRepository(GalleryDataDbContext context)
		{
			this.context = context;
		}

		public IEnumerable<Image> Get()
		{
			return context.Images.OrderBy(x => x.Title);
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
			context.Images.Add(entity);
		}

		public void Create(string title)
		{
			if (context.Images.Count(x => x.Title == title.Trim()) == 0)
				context.Images.Add(new Image() { Title = title.Trim() });
		}

		public void Update(int id, Image entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			Image oldImage = context.Images.FirstOrDefault(x => x.Id == id);
			if (entity == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldImage.Title = entity.Title;
			oldImage.Url = entity.Url;

			if (entity.Tags != null)
				oldImage.Tags = entity.Tags;
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
	}
}
