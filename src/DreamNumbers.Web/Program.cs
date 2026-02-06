using DreamNumbers.Extensions.Configuration;
using DreamNumbers.Storages.EFCore.SQLServer.DbContexts;
using DreamNumbers.Storages.EFCore.SQLServer.Extensions.Configuration;
using DreamNumbers.UI.Services;
using DreamNumbers.Web.Components;
using DreamNumbers.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace DreamNumbers.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddDreamNumbersCore();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("DefaultConnection is not defined.");

            builder.Services.AddAzureSqlDreamNumbersStorage(connectionString);

            // Add device-specific services used by the DreamNumbers.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddAdditionalAssemblies(
                    typeof(UI._Imports).Assembly);

            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DreamNumbersDbContext>();

                context.Database.Migrate();
            }

            app.Run();
        }
    }
}
