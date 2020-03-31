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
		public void Save(Image image);
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

		public async void Save(Image image)
		{
			if (image == null)
				throw new ArgumentNullException();

			if (image.Id == 0)
				imageRepository.Create(image);
			else imageRepository.Update(image.Id, image);
			await imageRepository.SaveAsync();
		}
	}
}
