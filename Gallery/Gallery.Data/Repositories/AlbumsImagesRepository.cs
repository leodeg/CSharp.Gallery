using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gallery.Data.Repositories
{
	public class AlbumsImagesRepository : Repository<AlbumImage>
	{
		public AlbumsImagesRepository(GalleryDataDbContext context) : base(context)
		{
		}
	}
}