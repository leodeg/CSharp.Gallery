using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Gallery.Data.Models
{
	public class AlbumImage
	{
		[ForeignKey(name: nameof(Album))]
		public int AlbumId { get; set; }
		public virtual Album Album { get; set; }

		[ForeignKey(name: nameof(Image))]
		public int ImageId { get; set; }
		public virtual Image Image { get; set; }
	}
}