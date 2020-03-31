using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gallery.Data.Models
{
	public class Image : Model
	{
		[MaxLength(50)]
		public string Title { get; set; }
		public string Url { get; set; }
		public virtual IEnumerable<Tag> Tags { get; set; }
	}
}