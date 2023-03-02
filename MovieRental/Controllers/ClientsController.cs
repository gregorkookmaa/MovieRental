using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using MovieRental.Models.ViewModels;
using MovieRental.Services;

namespace MovieRental.Controllers
{
    public class ClientsController : BaseController
    {
        private readonly IClientService _clientService;
        private const int pagesize = 5;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "firstName_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "lastName_asc" ? "lastName_desc" : "lastName_asc";
            ViewData["CurrentFilter"] = searchString;

            var model = await _clientService.GetPagedList(page, pagesize, searchString, sortOrder);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientService
               .GetById(id.Value);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            var model = new ClientEditModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientEditModel client)
        {
           return await Save(client);
        }

        [NonAction]
        private async Task<IActionResult> Save(ClientEditModel client)
        {
            var response = await _clientService.Save(client);
            if (!response.Success)
            {
                AddModelErrors(response);
                return View(client);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetById(id.Value);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(ClientEditModel model)
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
                await _clientService.Save(model);
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

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetById(id.Value);

            if (client == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _clientService.GetById(id);
            if (client == null)
            {
                return NotFound();
            }

            try
            {
                await _clientService.Delete(client);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ClientExists(int id)
        {
            return _clientService.GetById(id) != null;
        }
    }
}
