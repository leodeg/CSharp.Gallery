using Gallery.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gallery.Models
{
	public class UploadImageModel
	{
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		public string Tags { get; set; }
		public string Url { get; set; }

		public IEnumerable<Album> AllAlbums { get; set; }
		public IEnumerable<string> Albums { get; set; }
	}
}
