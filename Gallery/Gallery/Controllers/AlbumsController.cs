using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gallery.Data.Models;
using Gallery.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Gallery.Controllers
{
	public class AlbumsController : Controller
	{
		private readonly AlbumsRepository albumsRepository;
		private readonly string AlbumForm = "AlbumForm";

		public AlbumsController(AlbumsRepository albumsRepository)
		{
			this.albumsRepository = albumsRepository;
		}

		public IActionResult Index()
		{
			return View(albumsRepository.Get().OrderBy(a => a.Title));
		}

		public IActionResult Create()
		{
			return View(AlbumForm, new Album());
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var album = albumsRepository.Get(id.Value);
			if (album == null)
				return NotFound();

			return View(AlbumForm, album);
		}

		public async Task<IActionResult> Save(Album album)
		{
			if (!ModelState.IsValid)
				return View(AlbumForm, album);

			try
			{
				if (album.Id == 0)
					albumsRepository.Create(album);
				else albumsRepository.Update(album.Id, album);

				await albumsRepository.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// TODO: show warning about duplicate
				return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			albumsRepository.Remove(id.Value);
			await albumsRepository.SaveChangesAsync();
			return View(new Album());
		}
	}
}