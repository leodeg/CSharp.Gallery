using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gallery.Controllers
{
	public class TagsController : Controller
	{
		private readonly TagRepository tagRepository;

		public TagsController(TagRepository tagRepository)
		{
			this.tagRepository = tagRepository;
		}

		public IActionResult Index()
		{
			return View(tagRepository.Get());
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
				return NotFound();

			var tag = tagRepository.Get(id.Value);
			if (tag == null)
				return NotFound();

			return View(tag);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			tagRepository.Remove(id.Value);
			await tagRepository.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}