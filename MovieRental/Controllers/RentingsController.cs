using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using MovieRental.Models.ViewModels;
using MovieRental.Services;

namespace MovieRental.Controllers
{
    public class RentingsController : BaseController
    {
        private readonly IRentingService _rentingService;
        private const int pagesize = 10;

        public RentingsController(IRentingService rentingService)
        {
            _rentingService =rentingService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewData["NameSortParm"] = sortOrder == "fullName_asc" ? "fullName_desc" : "fullName_asc";
            ViewData["CurrentFilter"] = searchString;

            var model = await _rentingService.GetPagedList(page, pagesize, searchString, sortOrder);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        // GET: Rentings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renting = await _rentingService
               .GetById(id.Value);

            if (renting == null)
            {
                return NotFound();
            }

            return View(renting);
        }

        // GET: Rentings/Create
        public async Task<IActionResult> Create()
        {
            var model = new RentingEditModel();

            await _rentingService.FillEditModel(model);

            return View(model);
        }

        // POST: Rentings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentingEditModel renting)
        {
            return await Save(renting);
        }

        [NonAction]
        private async Task<IActionResult> Save(RentingEditModel renting)
        {
            if (!ModelState.IsValid)
            {
                await _rentingService.FillEditModel(renting);

                return View(renting);
            }

            var response = await _rentingService.Save(renting);
            if (!response.Success)
            {
                AddModelErrors(response);
                await _rentingService.FillEditModel(renting);

                return View(renting);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Rentings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renting = await _rentingService.GetForEdit(id.Value);
            if (renting == null)
            {
                return NotFound();
            }

            return View(renting);
        }

        // POST: Rentings/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(RentingEditModel model)
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
                await _rentingService.Save(model);
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

        // GET: Rentings/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renting = await _rentingService.GetById(id.Value);

            if (renting == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(renting);
        }

        // POST: Rentings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var renting = await _rentingService.GetById(id);
            if (renting == null)
            {
                return NotFound();
            }

            try
            {
                await _rentingService.Delete(renting);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool RentingExists(int id)
        {
            return _rentingService.GetById(id) != null;
        }
    }
}
