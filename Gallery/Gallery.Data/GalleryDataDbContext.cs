using Gallery.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gallery.Data
{
	public class GalleryDataDbContext : DbContext
	{
		public GalleryDataDbContext(DbContextOptions<GalleryDataDbContext> options) : base(options)
		{

		}

		public DbSet<Image> Images { get; set; }
		public DbSet<Tag> Tags { get; set; }
	}
}
