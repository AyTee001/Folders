using Folders.DAL.Entities;

namespace Folders.Interfaces
{
    public interface IFolderService
    {
        public Task<Folder> GetPrincipalFolder();
        public Task<Folder> GetFolder(long folderId);
    }
}
