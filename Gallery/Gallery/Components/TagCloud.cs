using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Components
{
	public class TagCloud : ViewComponent
	{
		private readonly TagRepository tagRepository;

		public TagCloud(TagRepository tagRepository)
		{
			this.tagRepository = tagRepository;
		}

		public IViewComponentResult Invoke()
		{
			var tags = tagRepository.Get();
			return View(tags);
		}
	}
}