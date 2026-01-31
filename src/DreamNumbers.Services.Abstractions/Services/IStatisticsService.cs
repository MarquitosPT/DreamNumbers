using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface IStatisticsService
    {
        List<NumberStatistics> CalculateStatistics(List<Draw> draws);
    }

}
