using Folders.Interfaces;
using Folders.Models;
using Microsoft.AspNetCore.Mvc;

namespace Folders.Controllers
{
    public class HomeController(IFolderService folderService) : Controller
    {
        private readonly IFolderService _folderService = folderService;

        public async Task<IActionResult> Index()
        {
            return View(await _folderService.GetPrincipalFolderAsync());
        }

        public IActionResult ChangeFolders()
        {
            return View(new FolderManagementModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetFolder(long id)
        {
            return View(nameof(Index), await _folderService.GetFolderAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> NewFoldersFromPC(FolderManagementModel model)
        {
            try
            {
                await _folderService.ImportFoldersFromPCAndSaveAsync(model.FolderForScanPath);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Error), new ErrorViewModel() { Message = "Invalid path input" });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> NewFoldersFromFile(IFormFile file)
        {
            try
            {
                await _folderService.ReadFromFileAndSaveAsync(file);
            }
            catch (Exception)
            {
                return Json(new { success = false, redirectUrl = Url.Action(nameof(Error), new ErrorViewModel() { Message = "Inapropriate file or content" }) });
            }
            return Json(new { success = true, redirectUrl = Url.Action(nameof(Index)) });
        }


        public async Task<IActionResult> DownloadFile()
        {
            return await _folderService.WriteToFileAsync();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }
    }
}
