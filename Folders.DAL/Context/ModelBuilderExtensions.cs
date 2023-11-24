using Folders.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Folders.DAL.Context
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureEntities(this ModelBuilder builder)
        {
            builder.Entity<Folder>().
                HasOne(e => e.Parent).
                WithMany(e => e.Children).
                HasForeignKey(e => e.ParentId);
        }
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Folder>().HasData([
                new Folder("Creating Digital Images") { Id = 1, ParentId = null },
                new Folder("Resources") { Id = 2, ParentId = 1 },
                new Folder("Evidence") { Id = 3, ParentId = 1 },
                new Folder("Graphic Products") { Id = 4, ParentId = 1 },
                new Folder("Primary Sources") { Id = 5, ParentId = 2 },
                new Folder("Secondary Sources") { Id = 6, ParentId = 2 },
                new Folder("Process") { Id = 7, ParentId = 4 },
                new Folder("Final Product") { Id = 8, ParentId = 4 },
            ]);
        }
    }
}
