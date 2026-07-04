namespace DreamNumbers.Services.JSC.Services
{
    using System.Text.RegularExpressions;
    using DreamNumbers.Services.JSC.Models;
    using HtmlAgilityPack;

    public class EuroMillionsScraper
    {
        private readonly HttpClient _http;

        public EuroMillionsScraper(HttpClient http)
        {
            _http = http;
        }

        public async Task<EuroMillionsResult?> GetResultAsync(int contestNumber, DateTime nextDate, int nextDrawIndex)
        {
            int maxContestGap = 500; // Número máximo de contest que pode tentar buscar antes de desistir
            int counter = 0;
            do
            {
                contestNumber++; // Incrementa o número do concurso para buscar o próximo sorteio
                counter++;

                string url = $"https://www.jogossantacasa.pt/web/SCCartazResult/euroMilhoes?selectContest={contestNumber}";
                string html;
                try
                {
                    html = await _http.GetStringAsync(url);

                    await Task.Delay(Random.Shared.Next(500, 2000)); // Atraso para evitar bloqueios por scraping
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
                var dateNode = doc.DocumentNode.SelectSingleNode("//span[@class='dataInfo']");
                if (dateNode == null)
                    continue;

                DateTime parsedDate = ParseDateNode(dateNode.InnerText.Trim());

                if (parsedDate != nextDate)
                    return null;

                // 2) Números principais
                var numberNodes = doc.DocumentNode.SelectSingleNode("//ul[@class='colums']//li");
                if (numberNodes == null)
                    return null;

                var numbers = ParseNumbersNode(numberNodes.InnerText.Trim()).ToList();

                // 3) Números das estrelas
                var starNodes = doc.DocumentNode.SelectSingleNode("//ul[@class='colums']//li");
                if (starNodes == null)
                    return null;

                var stars = ParseStarsNode(starNodes.InnerText.Trim()).ToList();

                // 4) Número do sorteio (incremental por ano)
                string drawNumber = $"{nextDrawIndex:000}/{parsedDate.Year}";

                return new EuroMillionsResult
                {
                    Date = parsedDate,
                    DrawNumber = drawNumber,
                    Numbers = numbers,
                    Stars = stars,
                    ContestNumber = contestNumber
                };

            } while (counter < maxContestGap);

            return null; // Não encontrou sorteio válido dentro do limite de tentativas
        }

        private static DateTime ParseDateNode(string nodeText)
        {
            Match matchDataSorteio = Regex.Match(input: nodeText, pattern: @"Data do sorteio - (\d*)[\-/](\d*)[\-/](\d*)", options: RegexOptions.IgnoreCase);

            string data = matchDataSorteio.Value.Replace("Data do Sorteio - ", "");

            return DateTime.Parse(data, System.Globalization.CultureInfo.GetCultureInfo("pt"));
        }

        private static IEnumerable<int> ParseNumbersNode(string nodeText)
        {
            Match matchSorteio = Regex.Match(input: nodeText, pattern: @"(\d\d|\d) (\d\d|\d) (\d\d|\d) (\d\d|\d) (\d\d|\d) \+ (\d\d|\d) (\d\d|\d)", options: RegexOptions.IgnoreCase);
            string sorteio = matchSorteio.Value.Replace(" + ", "|");

            var nodes = sorteio.Split('|', StringSplitOptions.RemoveEmptyEntries);

            foreach (var node in nodes[0].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(node.Trim(), out int number))
                {
                    yield return number;
                }
            }
        }

        private static IEnumerable<int> ParseStarsNode(string nodeText)
        {
            Match matchSorteio = Regex.Match(input: nodeText, pattern: @"(\d\d|\d) (\d\d|\d) (\d\d|\d) (\d\d|\d) (\d\d|\d) \+ (\d\d|\d) (\d\d|\d)", options: RegexOptions.IgnoreCase);
            string sorteio = matchSorteio.Value.Replace(" + ", "|");

            var nodes = sorteio.Split('|', StringSplitOptions.RemoveEmptyEntries);

            foreach (var node in nodes[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(node.Trim(), out int number))
                {
                    yield return number;
                }
            }
        }
    }
}
