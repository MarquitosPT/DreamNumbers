using DreamNumbers.Services;
using Marquitos.Schedulers;

namespace DreamNumbers.ScheduledTasks
{
    internal class DrawUpdateTask(IDrawUpdateService updateService) : IScheduledTask
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await updateService.UpdateDrawsAsync();
        }
    }

}
