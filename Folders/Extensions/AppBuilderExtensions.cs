using Folders.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Folders.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void UseFolderAppContext(this IApplicationBuilder app) 
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            using var context = scope?.ServiceProvider.GetRequiredService<FolderAppContext>();
            context?.Database.Migrate();
        }
    }
}
