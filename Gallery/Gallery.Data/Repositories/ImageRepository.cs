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
	public class ImageRepository : Repository<Image>
	{
		public ImageRepository(GalleryDataDbContext context) : base(context)
		{
		}

		public IEnumerable<Image> GetWithTag(string tag)
		{
			return dbSet.Where(c => c.Tags.Contains(tag)).OrderBy(x => x.Title);
		}

		public Image Get(string title)
		{
			return dbSet.SingleOrDefault(x => x.Title == title);
		}

		public void Create(string title)
		{
			dbSet.Add(new Image() { Title = title.Trim() });
		}

		public override void Update(int id, Image entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			Image oldImage = dbSet.FirstOrDefault(x => x.Id == id);
			if (entity == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldImage.Title = entity.Title;
			oldImage.Url = entity.Url;
			oldImage.Tags = entity.Tags;
		}

		public bool Remove(string title)
		{
			Image image = this.Get(title);
			if (image != null)
			{
				dbSet.Remove(image);
				return true;
			}
			return false;
		}
	}
}
