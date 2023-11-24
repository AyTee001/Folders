using Folders.DAL.Context;
using Folders.DAL.Entities;
using Folders.Exceptions;
using Folders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Folders.Services
{
    public class FolderService(FolderAppContext context) : IFolderService
    {
        private readonly FolderAppContext _context = context;
        public async Task<Folder> GetPrincipalFolder()
        {
            var folder = await _context.Folders
                .Include(e => e.Children)
                .SingleOrDefaultAsync(x => x.ParentId == null);

            return folder ?? throw new NotFoundException(nameof(folder));
        }

        public async Task<Folder> GetFolder(long folderId)
        {
            var folder = await _context.Folders
                .Include(e => e.Children)
                .SingleOrDefaultAsync(x => x.Id == folderId);

            return folder ?? throw new NotFoundException(nameof(folder), folderId);
        }
    }
}
