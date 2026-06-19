using DreamNumbers.Services;
using Marquitos.Schedulers;

namespace DreamNumbers.ScheduledTasks
{
    internal class DrawUpdateTask(IEnumerable<IDrawUpdateService> updateServices) : IScheduledTask
    {
        private readonly IEnumerable<IDrawUpdateService> _updateServices = updateServices;

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var updateService in _updateServices)
            {
                await updateService.UpdateDrawsAsync();
            }
        }
    }

}
