using Folders.DAL.Context;
using Folders.Extensions;
using Folders.Interfaces;
using Folders.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Folders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<FolderAppContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("FolderAppDBConnection"),
                    opt => opt.MigrationsAssembly(typeof(FolderAppContext).Assembly.GetName().Name)));

            builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

            builder.Services.AddScoped<IFolderService, FolderService>();

            var app = builder.Build();

            app.UseFolderAppContext();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
