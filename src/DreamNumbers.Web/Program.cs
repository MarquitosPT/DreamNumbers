using DreamNumbers.Extensions.Configuration;
using DreamNumbers.Storages.EFCore.SQLite.Extensions.Configuration;
using DreamNumbers.UI.Services;
using DreamNumbers.Web.Components;
using DreamNumbers.Web.Services;

namespace DreamNumbers
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
            builder.Services.AddDreamNumbersStorage("Data Source=data\\dreamnumbers.db");

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
                    typeof(DreamNumbers.UI._Imports).Assembly);

            app.Run();
        }
    }
}
