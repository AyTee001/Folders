using Folders.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Folders.DAL.Context
{
    public class FolderAppContext(DbContextOptions<FolderAppContext> options) : DbContext(options)
    {
        public DbSet<Folder> Folders => Set<Folder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureEntities();

            modelBuilder.Seed();
        }
    }
}
