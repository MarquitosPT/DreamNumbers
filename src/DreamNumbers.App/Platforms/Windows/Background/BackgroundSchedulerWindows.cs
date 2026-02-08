using DreamNumbers.Services;
using Windows.System.Threading;

namespace DreamNumbers.Platforms.Windows.Background;

public class BackgroundSchedulerWindows : IBackgroundScheduler
{
    private readonly IServiceProvider _serviceProvider;
    private ThreadPoolTimer? _timer;

    public BackgroundSchedulerWindows(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ScheduleDrawUpdate()
    {
        _timer = ThreadPoolTimer.CreatePeriodicTimer(async _ =>
        {
            using var scope = _serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IDrawUpdateService>();
            await service.UpdateDrawsAsync();

        }, TimeSpan.FromHours(1));

        _ = Task.Run(async () =>
        {
            using var scope = _serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IDrawUpdateService>();
            await service.UpdateDrawsAsync();
        });
    }
}
