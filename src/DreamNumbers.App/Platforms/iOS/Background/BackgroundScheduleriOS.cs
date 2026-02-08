using BackgroundTasks;
using DreamNumbers.Helpers;
using DreamNumbers.Services;
using Foundation;

namespace DreamNumbers.Platforms.iOS.Background;

public class BackgroundScheduleriOS : IBackgroundScheduler
{
    private const string TaskId = "com.dreamnumbers.drawupdate";

    public BackgroundScheduleriOS()
    {
        BGTaskScheduler.Shared.Register(TaskId, null, HandleTask);
    }

    public void ScheduleDrawUpdate()
    {
        var request = new BGAppRefreshTaskRequest(TaskId)
        {
            EarliestBeginDate = NSDate.Now.AddSeconds(60) // 1 minuto após arranque
        };

        BGTaskScheduler.Shared.Submit(request, out var error);
        // Podes logar o error se quiseres
    }

    private async void HandleTask(BGTask task)
    {
        try
        {
            using var scope = ServiceHelper.GetService<IServiceScopeFactory>().CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IDrawUpdateService>();
            await service.UpdateDrawsAsync();

            task.SetTaskCompleted(success: true);
        }
        catch
        {
            task.SetTaskCompleted(success: false);
        }

        // Reagendar para daqui a 1 hora
        var next = new BGAppRefreshTaskRequest(TaskId)
        {
            EarliestBeginDate = NSDate.Now.AddSeconds(3600) // 1 hora após a última execução
        };

        BGTaskScheduler.Shared.Submit(next, out var error);
    }
}
