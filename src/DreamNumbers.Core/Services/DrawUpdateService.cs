using DreamNumbers.Models;
using DreamNumbers.Services.EUDreams.Services;
using DreamNumbers.Storages;

namespace DreamNumbers.Services
{
    public class DrawUpdateService : IDrawUpdateService
    {
        private readonly EuroDreamsScraper _scraper;
        private readonly IDrawStorage _storage;

        public DrawUpdateService(EuroDreamsScraper scraper, IDrawStorage storage)
        {
            _scraper = scraper;
            _storage = storage;
        }

        public async Task UpdateDrawsAsync()
        {
            var lastDraw = await _storage.GetLastDrawAsync();

            DateTime startDate;
            int drawIndex;

            if (lastDraw == null)
            {
                startDate = new DateTime(2023, 11, 06);
                drawIndex = 0;
            }
            else
            {
                startDate = lastDraw.Date.AddDays(1);
                drawIndex = ExtractDrawIndex(lastDraw.DrawNumber);
            }

            DateTime today = DateTime.Today;

            for (var date = startDate; date <= today; date = date.AddDays(1))
            {
                if (date.Year != startDate.Year)
                    drawIndex = 0;

                if ((date.DayOfWeek != DayOfWeek.Monday) && (date.DayOfWeek != DayOfWeek.Thursday))
                    continue;

                var result = await _scraper.GetResultAsync(date, drawIndex + 1);

                if (result == null)
                    continue;

                drawIndex++;

                var draw = new Draw
                {
                    Date = result.Date,
                    DrawNumber = result.DrawNumber,
                    Numbers = result.Numbers,
                    DreamNumber = result.DreamNumber
                };

                await _storage.InsertAsync(draw);

                startDate = result.Date;
            }
        }

        private int ExtractDrawIndex(string drawNumber)
        {
            var parts = drawNumber.Split('/');
            return int.Parse(parts[0]);
        }
    }
}
