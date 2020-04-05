using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Gallery.Data.Models
{
	public class Album : Model
	{
		[Required]
		public string Title { get; set; }
		public virtual IEnumerable<AlbumImage> AlbumImages { get; set; }
	}
}