using DreamNumbers.Models;

namespace DreamNumbers.Storages
{
    public interface IDrawStorage
    {
        Task<List<Draw>> GetAllAsync();
        Task AddOrUpdateAsync(Draw draw);

        Task<Draw?> GetLastDrawAsync();
        Task<DateTime?> GetLastDrawDateAsync();
        Task InsertAsync(Draw draw);
        Task InsertManyAsync(IEnumerable<Draw> draws);

    }


}
