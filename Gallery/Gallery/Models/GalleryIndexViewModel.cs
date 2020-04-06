using Gallery.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Models
{
	public class GalleryIndexViewModel
	{
		public IEnumerable<Image> Images { get; set; }
		public PagingInformation PagingInformation { get; set; }
	}
}
