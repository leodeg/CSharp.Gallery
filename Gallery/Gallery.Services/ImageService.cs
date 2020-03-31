using Gallery.Data.Models;
using Gallery.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gallery.Services
{
	public interface IImageService
	{
		public Image GetById(int id);
		public IEnumerable<Image> GetAll();
		public IEnumerable<Image> GetWithTag(string tag);
	}

	public class ImageService : IImageService
	{
		private readonly IImageRepository imageRepository;

		public ImageService(IImageRepository imageRepository)
		{
			this.imageRepository = imageRepository;
		}

		public IEnumerable<Image> GetAll()
		{
			return imageRepository.Get();
		}

		public Image GetById(int id)
		{
			return imageRepository.Get(id);
		}

		public IEnumerable<Image> GetWithTag(string tag)
		{
			return imageRepository.GetWithTag(tag);
		}
	}
}
