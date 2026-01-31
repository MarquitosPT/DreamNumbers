using DreamNumbers.Models;
using DreamNumbers.Storages;

namespace DreamNumbers.Services
{
    internal class DrawUpdateService(IDrawStorage drawStorage, IHttpClientFactory httpClientFactory) : IDrawUpdateService
    {
        public async Task UpdateDrawsAsync()
        {
            var client = httpClientFactory.CreateClient();
            var html = await client.GetStringAsync("https://example.com/draws");

            var parsedDraws = ParseDraws(html);

            foreach (var draw in parsedDraws)
                await drawStorage.AddOrUpdateAsync(draw);
        }

        private List<Draw> ParseDraws(string html)
        {
            // TODO: implement parsing
            return new List<Draw>();
        }
    }

}
