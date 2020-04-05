using Gallery.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gallery.Models
{
	public class AlbumsViewModel
	{
		public IEnumerable<AlbumWithImage> Albums { get; set; }
	}

	public class AlbumWithImage
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int ImageCount { get; set; }
		public string ImageUrl { get; set; }
	}
}
