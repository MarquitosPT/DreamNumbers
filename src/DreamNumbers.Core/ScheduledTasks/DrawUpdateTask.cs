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
                try
                {
                    await updateService.UpdateDrawsAsync();
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    Console.WriteLine($"Error updating draws for {updateService.GetType().Name}: {ex.Message}");
                }
            }
        }
    }

}
