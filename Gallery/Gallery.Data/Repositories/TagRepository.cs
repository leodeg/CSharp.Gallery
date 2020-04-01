using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class TagRepository : ITagRepository
	{
		private GalleryDataDbContext context;

		public TagRepository(GalleryDataDbContext context)
		{
			this.context = context;
		}

		public IEnumerable<Tag> Get()
		{
			return context.Tags.OrderBy(x => x.Title);
		}

		public Tag Get(int id)
		{
			return context.Tags.FirstOrDefault(x => x.Id == id);
		}

		public Tag Get(string title)
		{
			return context.Tags.SingleOrDefault(x => x.Title == title);
		}

		public void Create(Tag entity)
		{
			entity.Created = DateTime.Now;
			context.Tags.Add(entity);
		}

		public void Create(string title)
		{
			if (context.Tags.Count(x => x.Title == title.Trim()) == 0)
			{
				context.Tags.Add(new Tag() { Title = title.Trim(), Created = DateTime.Now });
			}
		}

		public void Update(int id, Tag entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			Tag oldTag = context.Tags.FirstOrDefault(x => x.Id == id);
			if (entity == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldTag.Title = entity.Title;
		}

		public bool Delete(int id)
		{
			Tag tag = context.Tags.FirstOrDefault(x => x.Id == id);
			if (tag != null)
			{
				context.Tags.Remove(tag);
				return true;
			}
			return false;
		}

		public bool Delete(string title)
		{
			Tag tagToDelete = Get(title);
			if (tagToDelete != null)
			{
				context.Tags.Remove(tagToDelete);
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
