using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using MovieRental.Models.ViewModels;
using MovieRental.Services;

namespace MovieRental.Controllers
{
    public class RentingMoviesController : BaseController
    {
        private readonly IRentingMovieService _rentingMovieService;
        private const int pagesize = 10;

        public RentingMoviesController(IRentingMovieService rentingMovieService)
        {
            _rentingMovieService = rentingMovieService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewData["ClientNameSortParm"] = sortOrder == "clientName_asc" ? "clientName_desc" : "clientName_asc";
            ViewData["CurrentFilter"] = searchString;

            var model = await _rentingMovieService.GetPagedList(page, pagesize, searchString, sortOrder);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        // GET: RentingMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentingMovie = await _rentingMovieService
               .GetById(id.Value);

            if (rentingMovie == null)
            {
                return NotFound();
            }

            return View(rentingMovie);
        }

        // GET: RentingMovies/Create
        public async Task<IActionResult> Create()
        {
            var model = new RentingMovieEditModel();

            await _rentingMovieService.FillEditModel(model);

            return View(model);
        }

        // POST: RentingMovies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentingMovieEditModel rentingMovie)
        {
            return await Save(rentingMovie);
        }

        [NonAction]
        private async Task<IActionResult> Save(RentingMovieEditModel rentingMovie)
        {
            if (!ModelState.IsValid)
            {
                await _rentingMovieService.FillEditModel(rentingMovie);

                return View(rentingMovie);
            }

            var response = await _rentingMovieService.Save(rentingMovie);
            if (!response.Success)
            {
                AddModelErrors(response);
                await _rentingMovieService.FillEditModel(rentingMovie);

                return View(rentingMovie);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: RentingMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentingMovie = await _rentingMovieService.GetForEdit(id.Value);
            if (rentingMovie == null)
            {
                return NotFound();
            }
            
            return View(rentingMovie);
        }

        // POST: RentingMovies/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(RentingMovieEditModel model)
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
                await _rentingMovieService.Save(model);
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

        // GET: RentingMovies/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentingMovie = await _rentingMovieService.GetById(id.Value);

            if (rentingMovie == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(rentingMovie);
        }

        // POST: RentingMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentingMovie = await _rentingMovieService.GetById(id);

            if (rentingMovie == null)
            {
                return NotFound();
            }

            try
            {
                await _rentingMovieService.Delete(rentingMovie);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool RentingMovieExists(int id)
        {
            return _rentingMovieService.GetById(id) != null;
        }
    }
}
