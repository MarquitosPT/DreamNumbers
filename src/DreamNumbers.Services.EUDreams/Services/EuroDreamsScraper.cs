namespace DreamNumbers.Services.EUDreams.Services
{
    using DreamNumbers.Services.EUDreams.Models;
    using HtmlAgilityPack;

    public class EuroDreamsScraper
    {
        private readonly HttpClient _http;

        public EuroDreamsScraper(HttpClient http)
        {
            _http = http;
        }

        public async Task<EuroDreamsResult?> GetResultAsync(DateTime date, int drawIndex)
        {
            string urlDate = date.ToString("dd-MM-yyyy");
            string url = $"https://eu-dreams.com/pt/resultados/{urlDate}";

            string html;

            try
            {
                html = await _http.GetStringAsync(url);

                await Task.Delay(Random.Shared.Next(1000,10000)); // Atraso para evitar bloqueios por scraping
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Página não existe → sorteio inexistente
                    return null;
                }

                throw; // Erro inesperado, propagar
            }
            catch
            {
                // Página não existe → sorteio inexistente
                return null;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // 1) Data do sorteio
            var dateNode = doc.DocumentNode.SelectSingleNode("//div[@class='h3']");
            if (dateNode == null)
                return null;

            DateTime parsedDate = DateTime.Parse(dateNode.InnerText.Trim());

            // 2) Números principais
            var numberNodes = doc.DocumentNode.SelectNodes("//li[@class='ball ball']");
            if (numberNodes == null)
                return null;

            var numbers = numberNodes
                .Select(n => int.Parse(n.InnerText.Trim()))
                .ToList();

            // 3) Número Dream
            var dreamNode = doc.DocumentNode.SelectSingleNode("//li[@class='ball dream-number']");
            if (dreamNode == null)
                return null;

            int dream = int.Parse(dreamNode.InnerText.Trim());

            // 4) Número do sorteio (incremental por ano)
            string drawNumber = $"{drawIndex:000}/{parsedDate.Year}";

            return new EuroDreamsResult
            {
                Date = parsedDate,
                DrawNumber = drawNumber,
                Numbers = numbers,
                DreamNumber = dream
            };
        }
    }
}
