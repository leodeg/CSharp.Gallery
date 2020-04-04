using Gallery.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gallery.Data
{
	public class GalleryDataDbContext : DbContext
	{
		public DbSet<Image> Images { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Album> Albums { get; set; }
		public DbSet<AlbumImage> AlbumImages { get; set; }

		public GalleryDataDbContext(DbContextOptions<GalleryDataDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Default Created Value
			modelBuilder.Entity<Album>().Property(a => a.Created).HasDefaultValueSql("GETDATE()");
			modelBuilder.Entity<Image>().Property(i => i.Created).HasDefaultValueSql("GETDATE()");
			modelBuilder.Entity<Tag>().Property(t => t.Created).HasDefaultValueSql("GETDATE()");

			// Columns with unique values
			modelBuilder.Entity<Album>().HasAlternateKey(a => a.Title);
			modelBuilder.Entity<Tag>().HasAlternateKey(t => t.Title);

			// Images' albums primary key
			modelBuilder.Entity<AlbumImage>()
				.HasKey(ai => new { ai.AlbumId, ai.ImageId });

			// Link for images
			modelBuilder.Entity<AlbumImage>()
				.HasOne(ai => ai.Image)
				.WithMany(ai => ai.AlbumImages)
				.HasForeignKey(ai => ai.ImageId);

			// Link for albums
			modelBuilder.Entity<AlbumImage>()
				.HasOne(ai => ai.Album)
				.WithMany(ai => ai.AlbumImages)
				.HasForeignKey(ai => ai.AlbumId);
		}
	}
}
