using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface IStatisticsService
    {
        List<NumberStatistics> CalculateMainNumberStatistics(IReadOnlyList<Draw> draws, int maxNumber);
        List<DreamNumberStatistics> CalculateDreamNumberStatistics(IReadOnlyList<Draw> draws, int maxDreamNumber);

        List<DashboardMainStatistics> CalculateDashboardMainStatistics(IReadOnlyList<Draw> draws, int maxNumber);
        List<DashboardDreamStatistics> CalculateDashboardDreamStatistics(IReadOnlyList<Draw> draws, int maxDreamNumber);
    }

}
