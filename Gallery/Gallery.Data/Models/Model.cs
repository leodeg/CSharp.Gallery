using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gallery.Data.Models
{
	public class Model
	{
		[Key]
		public int Id { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime Created { get; set; }
	}
}