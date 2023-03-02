using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MovieRental.FileAccess;
using MovieRental.Models;

namespace MovieRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileClient _fileClient;

        public HomeController(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile[] uploadedFiles)
        {
            if (uploadedFiles == null)
            {
                return BadRequest();
            }

            foreach (var uploadedFile in uploadedFiles)
            {
                using (var inputStream = uploadedFile.OpenReadStream())
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(uploadedFile.FileName);
                    _fileClient.Save(FileContainerNames.Documents, fileName, inputStream);
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
