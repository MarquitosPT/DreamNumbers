namespace DreamNumbers.Models
{
    public abstract class BaseNumberStatistics
    {
        public int Number { get; init; }

        /// <summary>Total de vezes que o número apareceu.</summary>
        public int Count { get; set; }

        /// <summary>Índice do último sorteio onde apareceu (0 = primeiro sorteio).</summary>
        public int LastSeenIndex { get; set; } = -1;

        /// <summary>Gap atual: sorteios desde a última aparição.</summary>
        public int Gap { get; set; }

        /// <summary>Lista de gaps históricos entre aparições consecutivas.</summary>
        public List<int> Gaps { get; } = new();

        /// <summary>Hazard rate por gap: P(aparecer | gap = g).</summary>
        public Dictionary<int, double> HazardRates { get; } = new();
    }
}
