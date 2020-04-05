using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Gallery.Data.Models
{
	public class Image : Model
	{
		[Required]
		[MaxLength(50)]
		public string Title { get; set; }

		[DisplayName("Image Path")]
		public string Url { get; set; }

		public string Tags { get; set; }

		public List<string> TagsList
		{
			get
			{
				if (!string.IsNullOrEmpty(Tags))
					return Tags.Split(",")
						.Select(tag => tag.Trim())
						.ToList();
				return null;
			}
		}

		public virtual IEnumerable<AlbumImage> AlbumImages { get; set; }
	}
}