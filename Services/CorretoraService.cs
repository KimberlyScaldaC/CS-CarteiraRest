namespace CS_CarteiraRest.Services
{
    public class CorretoraService
    {
        // Dicionário de moedas com valores fictícios
        private readonly Dictionary<string, decimal> _precos = new()
        {
            { "BTC", 250000m },   // 1 BTC = R$ 250.000
            { "ETH", 12000m },    // 1 ETH = R$ 12.000
            { "DOGE", 0.5m }      // 1 DOGE = R$ 0,50
        };

        public decimal GetPrecoAtual(string moeda)
        {
            moeda = moeda.ToUpper();

            if (_precos.ContainsKey(moeda))
            {
                return _precos[moeda];
            }

            throw new Exception($"Moeda '{moeda}' não suportada pela corretora.");
        }
    }
}
