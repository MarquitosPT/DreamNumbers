namespace DreamNumbers.Services.Background;

public class BackgroundSchedulerDefault : IBackgroundScheduler
{
    private Timer? _timer;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundSchedulerDefault(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ScheduleDrawUpdate()
    {
        // 1 minuto depois, depois de hora em hora – apenas enquanto a app está aberta
        _timer = new Timer(async _ =>
        {
            using var scope = _serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IDrawUpdateService>();
            await service.UpdateDrawsAsync();

        }, null, TimeSpan.FromMinutes(1), TimeSpan.FromHours(1));
    }
}
