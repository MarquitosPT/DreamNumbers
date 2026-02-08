using DreamNumbers.Extensions.Configuration;
using DreamNumbers.Helpers;
using DreamNumbers.Services;
using DreamNumbers.Services.Background;
using DreamNumbers.Storages.EFCore.SQLite.DbContexts;
using DreamNumbers.Storages.EFCore.SQLite.Extensions.Configuration;
using DreamNumbers.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
#if ANDROID
using DreamNumbers.Platforms.Android.Background;
#elif IOS
using DreamNumbers.Platforms.iOS.Background;
#elif WINDOWS
using DreamNumbers.Platforms.Windows.Background;
#endif

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

            var dbPath = DatabaseInitializer.InitializeDatabase();
            builder.Services.AddSqliteDreamNumbersStorage($"Data Source={dbPath}");

            // BackgroundScheduler por plataforma
#if ANDROID
                builder.Services.AddSingleton<IBackgroundScheduler, BackgroundSchedulerAndroid>();
#elif IOS
                builder.Services.AddSingleton<IBackgroundScheduler, BackgroundScheduleriOS>();
#elif WINDOWS
            builder.Services.AddSingleton<IBackgroundScheduler, BackgroundSchedulerWindows>();
            #else
                builder.Services.AddSingleton<IBackgroundScheduler, BackgroundSchedulerDefault>();
            #endif

            // Add device-specific services used by the DreamNumbers.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            ServiceHelper.Initialize(app.Services);

            var scheduler = app.Services.GetRequiredService<IBackgroundScheduler>();
            scheduler.ScheduleDrawUpdate();

            return app;
        }
    }
}
