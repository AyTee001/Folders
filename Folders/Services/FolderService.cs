using AutoMapper;
using Folders.Comparers;
using Folders.DAL.Context;
using Folders.DAL.Entities;
using Folders.DTOs;
using Folders.Exceptions;
using Folders.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Folders.Services
{
    public class FolderService(FolderAppContext context, IMapper mapper) : IFolderService
    {
        private static readonly JsonSerializerOptions _serializationOptions = new()
        {
            WriteIndented = true
        };

        private readonly FolderAppContext _context = context;

        private readonly IMapper _mapper = mapper;

        public async Task<Folder> GetPrincipalFolderAsync()
        {
            var folder = await _context.Folders
                .Include(e => e.Children)
                .SingleOrDefaultAsync(x => x.ParentId == null);

            return folder ?? throw new NotFoundException(nameof(folder));
        }

        public async Task<Folder> GetFolderAsync(long folderId)
        {
            var folder = await _context.Folders
                .Include(e => e.Children)
                .SingleOrDefaultAsync(x => x.Id == folderId);

            return folder ?? throw new NotFoundException(nameof(folder), folderId);
        }

        public async Task ImportFoldersFromPCAndSaveAsync(string? startPath)
        {

            if (string.IsNullOrWhiteSpace(startPath) || !Directory.Exists(startPath))
            {
                throw new ArgumentException("Invalid or non-existent start path.", nameof(startPath));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Folders.RemoveRange(_context.Folders);
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Folders', RESEED, 0)");

                List<Folder> folders = [];

                if (!Directory.Exists(startPath))
                {
                    throw new NotFoundException(nameof(startPath));
                }

                Queue<(string Path, long? ParentId)> queue = [];

                queue.Enqueue((startPath, null));

                while (queue.Count > 0)
                {
                    var (path, parentId) = queue.Dequeue();

                    var folder = new Folder(Path.GetFileName(path))
                    {
                        ParentId = parentId
                    };

                    _context.Folders.Add(folder);
                    folders.Add(folder);
                    await _context.SaveChangesAsync();

                    foreach (var subdirectory in Directory.GetDirectories(path))
                    {
                        queue.Enqueue((subdirectory, folder.Id));
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<FileStreamResult> WriteToFileAsync()
        {
            MemoryStream memoryStream = new();
            await JsonSerializer.SerializeAsync(memoryStream, _mapper.Map<List<FolderStorageDto>>(_context.Folders), _serializationOptions);

            memoryStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(memoryStream, "application/json")
            {
                FileDownloadName = $"folders_{DateTime.Now:yyyyMMddHHmmss}.txt"
            };
        }

        public async Task ReadFromFileAndSaveAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            stream.Position = 0;

            var folders = await JsonSerializer.DeserializeAsync<List<FolderStorageDto>>(stream);
            folders!.Sort((a, b) => NullableNumComparer.CompareNullable(a.ParentId, b.ParentId));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Folders.RemoveRange(_context.Folders);
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Folders', RESEED, 0)");

                Queue<(FolderStorageDto FolderDto, long? ParentId)> folderQueue = [];

                folderQueue.Enqueue((folders.Single(x => x.ParentId == null), null));

                while (folderQueue.Count > 0)
                {
                    var (folderDto, parentId) = folderQueue.Dequeue();

                    var newFolder = new Folder(folderDto.Name)
                    {
                        ParentId = parentId
                    };

                    _context.Folders.Add(newFolder);
                    await _context.SaveChangesAsync();

                    foreach (var childDto in folders.Where(f => f.ParentId == folderDto.Id))
                    {
                        folderQueue.Enqueue((childDto, newFolder.Id));
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<ICollection<FolderStorageDto>> GetAllFoldersAsync()
        {
            return _mapper.Map<ICollection<FolderStorageDto>>(await _context.Folders.ToListAsync());
        }
    }
}
