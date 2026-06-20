using DreamNumbers.Models;

namespace DreamNumbers.Storages
{
    public interface IEuroMillionDrawStorage
    {
        Task<List<EuroMillionDraw>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<EuroMillionDraw>> GetLastDrawsAsync(int count, CancellationToken cancellationToken = default);

        Task<EuroMillionDraw?> GetLastDrawAsync(CancellationToken cancellationToken = default);
        Task<DateTime?> GetLastDrawDateAsync(CancellationToken cancellationToken = default);
        Task AddOrUpdateAsync(EuroMillionDraw draw, CancellationToken cancellationToken = default);
        Task InsertAsync(EuroMillionDraw draw, CancellationToken cancellationToken = default);
        Task InsertManyAsync(IEnumerable<EuroMillionDraw> draws, CancellationToken cancellationToken = default);

    }


}
