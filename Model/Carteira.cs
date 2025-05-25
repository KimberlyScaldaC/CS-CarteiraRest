namespace CS_CarteiraRest.Model
{
    public class Carteira
    {
        // Construtor padrão: inicializa os campos com valores padrão
        public Carteira()
        {
            Nome = "";
            Moeda = "";
            Saldo = 0;
        }

        // Construtor com parâmetros: permite criar uma carteira com valores definidos
        public Carteira(string nome, string moeda, decimal saldo)
        {
            Nome = nome;
            Moeda = moeda;
            Saldo = saldo;
        }

        // Propriedade que representa o identificador único da carteira
        public int Id { get; set; }

        // Propriedade que representa o nome do dono ou identificação da carteira
        public string? Nome { get; set; }

        // Propriedade que representa o tipo de moeda associada à carteira
        public string? Moeda { get; set; }

        // Propriedade que representa o saldo atual da carteira
        public decimal Saldo { get; set; }
    }
}
