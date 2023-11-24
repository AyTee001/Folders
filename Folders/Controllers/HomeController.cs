using Folders.Interfaces;
using Folders.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Folders.Controllers
{
    public class HomeController(IFolderService folderService) : Controller
    {
        private readonly IFolderService _folderService = folderService;

        public async Task<IActionResult> Index()
        {
            return View(await _folderService.GetPrincipalFolder());
        }

        [HttpGet]
        public async Task<IActionResult> GetFolder(long id)
        {
            return View("Index", await _folderService.GetFolder(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
