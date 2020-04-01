using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gallery.Models
{
	public class UploadImageModel
	{
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		public string Tags { get; set; }
	}
}
