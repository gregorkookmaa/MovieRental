using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using MovieRental.Models.ViewModels;
using MovieRental.Services;

namespace MovieRental.Controllers
{
    public class MoviesController : BaseController
    {
        private readonly IMovieService _movieService;
        private const int pagesize = 10;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["GenreSortParm"] = sortOrder == "genre_asc" ? "genre_desc" : "genre_asc";
            ViewData["CurrentFilter"] = searchString;
            var model = await _movieService.GetPagedList(page, pagesize, searchString, sortOrder);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetById(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            var model = new MoviesEditModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoviesEditModel movie)
        {
            return await Save(movie);
        }

        [NonAction]
        private async Task<IActionResult> Save(MoviesEditModel movie)
        {
            var response = await _movieService.Save(movie);

            if (!response.Success)
            {
                AddModelErrors(response);
                return View(movie);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetById(id.Value);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(MoviesEditModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _movieService.Save(model);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }

            return View(model);
        }


        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetById(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            try
            {
                await _movieService.Delete(movie);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool MovieExists(int id)
        {
            return _movieService.GetById(id) != null;
        }
    }
}
