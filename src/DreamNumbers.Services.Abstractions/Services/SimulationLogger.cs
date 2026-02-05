using DreamNumbers.Models;

namespace DreamNumbers.Services
{

    public static class SimulationLogger
    {
        public static List<string> GenerateLogs(
            SimulationResult result,
            string strategyName,
            string profile,
            int interval)
        {
            var logs = new List<string>();

            foreach (var combo in result.Combinations)
            {
                var sb = new System.Text.StringBuilder();

                sb.AppendLine($"Estratégia: {strategyName}");
                sb.AppendLine($"Perfil: {profile}");
                sb.AppendLine($"Intervalo: últimos {interval} sorteios");
                sb.AppendLine();

                sb.AppendLine("Números selecionados:");
                foreach (var n in combo.Numbers)
                    sb.AppendLine($" - Número {n}");

                sb.AppendLine();
                sb.AppendLine($"DreamNumber selecionado: {combo.DreamNumber}");

                sb.AppendLine();
                sb.AppendLine("Notas:");
                sb.AppendLine(" - Os números foram escolhidos com base nos scores ponderados.");
                sb.AppendLine(" - A seleção é probabilística, mas influenciada pelos pesos do perfil.");
                sb.AppendLine(" - Estratégias como MedianGap, HazardRate e Trend influenciaram os scores.");

                logs.Add(sb.ToString());
            }

            return logs;
        }
    }
}
