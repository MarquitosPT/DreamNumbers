using DreamNumbers.Models;
using DreamNumbers.Services.JSC.Services;
using DreamNumbers.Storages;

namespace DreamNumbers.Services
{
    internal class EuroMillionsDrawUpdateService : IDrawUpdateService
    {
        private readonly IEuroMillionDrawStorage _drawStorage;
        private readonly EuroMillionsScraper _scraper;

        public EuroMillionsDrawUpdateService(EuroMillionsScraper scraper, IEuroMillionDrawStorage drawStorage)
        {
            _scraper = scraper;
            _drawStorage = drawStorage;
        }

        public async Task UpdateDrawsAsync()
        {
            var lastDraw = await _drawStorage.GetLastDrawAsync();

            DateTime startDate;
            int drawIndex;
            int contestNumber;

            if (lastDraw == null)
            {
                startDate = new DateTime(2023, 11, 06);
                drawIndex = 0;
                contestNumber = 7110;
            }
            else
            {
                startDate = lastDraw.Date.AddDays(1);
                drawIndex = ExtractDrawIndex(lastDraw.DrawNumber);
                contestNumber = lastDraw.ContestNumber;
            }

            DateTime today = DateTime.Today;

            for (var date = startDate; date < today; date = date.AddDays(1))
            {
                if (date.Year != startDate.Year)
                    drawIndex = 0;

                if ((date.DayOfWeek != DayOfWeek.Tuesday) && (date.DayOfWeek != DayOfWeek.Friday))
                    continue;

                var result = await _scraper.GetResultAsync(contestNumber, date, drawIndex + 1);

                if (result == null)
                    break;

                drawIndex++;

                var draw = new EuroMillionDraw
                {
                    Date = result.Date,
                    DrawNumber = result.DrawNumber,
                    Numbers = result.Numbers,
                    Stars = result.Stars,
                    ContestNumber = result.ContestNumber
                };

                await _drawStorage.InsertAsync(draw);

                startDate = result.Date;
            }
        }

        private static int ExtractDrawIndex(string drawNumber)
        {
            var parts = drawNumber.Split('/');
            return int.Parse(parts[0]);
        }
    }
}
