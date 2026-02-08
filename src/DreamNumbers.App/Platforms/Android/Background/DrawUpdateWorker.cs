using Android.Content;
using AndroidX.Work;
using DreamNumbers.Helpers;
using DreamNumbers.Services;

namespace DreamNumbers.Platforms.Android.Background;

public class DrawUpdateWorker : Worker
{
    public DrawUpdateWorker(Context context, WorkerParameters workerParameters)
        : base(context, workerParameters)
    {
    }

    public override Result DoWork()
    {
        try
        {
            using var scope = ServiceHelper.GetService<IServiceScopeFactory>().CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IDrawUpdateService>();
            service.UpdateDrawsAsync().GetAwaiter().GetResult();

            return Result.InvokeSuccess();
        }
        catch
        {
            return Result.InvokeRetry();
        }
    }
}
