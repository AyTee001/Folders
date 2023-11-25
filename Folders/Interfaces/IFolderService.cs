using Folders.DAL.Entities;
using Folders.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Folders.Interfaces
{
    public interface IFolderService
    {
        public Task<ICollection<FolderStorageDto>> GetAllFoldersAsync();
        public Task<Folder> GetPrincipalFolderAsync();
        public Task<Folder> GetFolderAsync(long folderId);
        public Task ImportFoldersFromPCAndSaveAsync(string? startPath);
        public Task<FileStreamResult> WriteToFileAsync();
        public Task ReadFromFileAndSaveAsync(IFormFile file);
    }
}
