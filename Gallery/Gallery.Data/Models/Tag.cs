using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gallery.Data.Models
{

	public class Tag : Model
	{
		public string Title { get; set; }
	}
}