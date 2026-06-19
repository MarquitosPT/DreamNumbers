using DreamNumbers.Models;

namespace DreamNumbers.Storages
{
    public interface IEuroMillionDrawStorage
    {
        Task<List<EuroMillionDraw>> GetAllAsync();
        Task<EuroMillionDraw?> GetLastDrawAsync();
        Task<DateTime?> GetLastDrawDateAsync();
        Task AddOrUpdateAsync(EuroMillionDraw draw);
        Task InsertAsync(EuroMillionDraw draw);
        Task InsertManyAsync(IEnumerable<EuroMillionDraw> draws);

    }


}
