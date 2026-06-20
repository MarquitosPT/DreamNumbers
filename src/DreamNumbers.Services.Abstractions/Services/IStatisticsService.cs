using DreamNumbers.Models;

namespace DreamNumbers.Services
{
    public interface IStatisticsService
    {
        List<NumberStatistics> CalculateMainNumberStatistics(IReadOnlyList<Draw> draws, int maxNumber);
        List<DreamNumberStatistics> CalculateDreamNumberStatistics(IReadOnlyList<Draw> draws, int maxDreamNumber);

        List<NumberStatistics> CalculateNumberStatistics(IReadOnlyList<EuroMillionDraw> draws, int maxNumber);
        List<StarStatistics> CalculateStarStatistics(IReadOnlyList<EuroMillionDraw> draws, int maxStarNumber);

        List<DashboardMainStatistics> CalculateDashboardMainStatistics(IReadOnlyList<Draw> draws, int maxNumber);
        List<DashboardDreamStatistics> CalculateDashboardDreamStatistics(IReadOnlyList<Draw> draws, int maxDreamNumber);

        List<DashboardMainStatistics> CalculateDashboardNumberStatistics(IReadOnlyList<EuroMillionDraw> draws, int maxNumber);
        List<DashboardStarStatistics> CalculateDashboardStarStatistics(IReadOnlyList<EuroMillionDraw> draws, int maxStarNumber);
    }

}
