using DreamNumbers.Models;

namespace DreamNumbers.Storages
{
    public interface IDrawStorage
    {
        Task<List<Draw>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Draw>> GetLastDrawsAsync(int count, CancellationToken cancellationToken = default);

        Task AddOrUpdateAsync(Draw draw, CancellationToken cancellationToken = default);

        Task<Draw?> GetLastDrawAsync(CancellationToken cancellationToken = default);
        Task<DateTime?> GetLastDrawDateAsync(CancellationToken cancellationToken = default);
        Task InsertAsync(Draw draw, CancellationToken cancellationToken = default);
        Task InsertManyAsync(IEnumerable<Draw> draws, CancellationToken cancellationToken = default);

    }


}
