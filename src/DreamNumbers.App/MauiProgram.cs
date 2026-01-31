using DreamNumbers.Extensions.Configuration;
using DreamNumbers.Services;
using DreamNumbers.Storages.EFCore.SQLite.Extensions.Configuration;
using DreamNumbers.UI.Services;
using Microsoft.Extensions.Logging;

namespace DreamNumbers
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddDreamNumbersCore();
            builder.Services.AddDreamNumbersStorage("Data Source=dreamnumbers.db");

            // Add device-specific services used by the DreamNumbers.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
