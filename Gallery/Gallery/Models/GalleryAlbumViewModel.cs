using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Models
{

	public class GalleryAlbumViewModel
	{
		public string AlbumTitle { get; set; }
		public IEnumerable<Image> Images { get; set; }
	}
}
