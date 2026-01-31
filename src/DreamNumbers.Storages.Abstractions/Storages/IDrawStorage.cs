using DreamNumbers.Models;

namespace DreamNumbers.Storages
{
    public interface IDrawStorage
    {
        Task<List<Draw>> GetAllAsync();
        Task AddOrUpdateAsync(Draw draw);
    }


}
