using AndroidX.Work;
using Application = Android.App.Application;

namespace DreamNumbers.Platforms.Android.Background;

public class BackgroundSchedulerAndroid : Services.IBackgroundScheduler
{
    public void ScheduleDrawUpdate()
    {
        var networktype = NetworkType.Connected ?? throw new Exception("NetworkType.Connected returned a null instance.");

        var constraints = new Constraints.Builder()
            .SetRequiredNetworkType(networktype)
            .Build();

        var builder = PeriodicWorkRequest.Builder
            .From<DrawUpdateWorker>(TimeSpan.FromHours(1));

        builder.SetInitialDelay(1, Java.Util.Concurrent.TimeUnit.Minutes);
        builder.SetConstraints(constraints);

        var request = builder.Build();

        var workPolice = ExistingPeriodicWorkPolicy.Update ?? throw new Exception("ExistingPeriodicWorkPolicy.Update returned a null instance");

        WorkManager.GetInstance(Application.Context)
            .EnqueueUniquePeriodicWork("DrawUpdate", ExistingPeriodicWorkPolicy.Update,
                (PeriodicWorkRequest)request);
    }
}
