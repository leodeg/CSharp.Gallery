using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class TagRepository : Repository<Tag>
	{
		public TagRepository(GalleryDataDbContext context) : base(context)
		{
		}

		public Tag Get(string title)
		{
			return context.Tags.SingleOrDefault(x => x.Title == title);
		}

		public void Create(string title)
		{
			if (context.Tags.Count(x => x.Title == title.Trim()) == 0)
			{
				context.Tags.Add(new Tag() { Title = title.Trim(), Created = DateTime.Now });
			}
		}

		public void Create(IEnumerable<string> titles)
		{
			foreach (string title in titles)
			{
				Create(title);
			}
		}

		public override void Update(int id, Tag entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			Tag oldTag = context.Tags.FirstOrDefault(x => x.Id == id);
			if (entity == null)
				throw new ArgumentOutOfRangeException("Can't find and update item with id: " + id);

			oldTag.Title = entity.Title;
		}

		public bool Remove(string title)
		{
			Tag tagToDelete = Get(title);
			if (tagToDelete != null)
			{
				context.Tags.Remove(tagToDelete);
				return true;
			}
			return false;
		}
	}
}
