using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface IStatisticsService
    {
        List<NumberStatistics> CalculateStatistics(List<Draw> draws);
        List<DreamNumberStatistics> CalculateDreamNumberStatistics(List<Draw> draws);
    }

}
