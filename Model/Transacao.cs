namespace CS_CarteiraRest.Model
{
    // Classe que representa uma transação financeira associada a uma carteira
    public class Transacao
    {
        // Construtor padrão: inicializa propriedades com valores padrão
        public Transacao()
        {
            Id = 0;
            IdCarteira = 0;
            Tipo = "";
            Valor = 0;
            Data = DateTime.Now; // Define a data da transação como o momento atual
        }

        // Construtor com parâmetros: permite criar uma transação com valores definidos
        public Transacao(int id, int idCarteira, string tipo, decimal valor, DateTime data)
        {
            Id = id;
            IdCarteira = idCarteira;
            Tipo = tipo;
            Valor = valor;
            Data = data;
        }

        // Identificador único da transação
        public int Id { get; set; }

        // Chave estrangeira que referencia a carteira relacionada
        public int IdCarteira { get; set; }

        // Valor monetário da transação
        public decimal Valor { get; set; }

        // Tipo da transação: "entrada", "saida" ou "transferencia"
        public string Tipo { get; set; }

        // Descrição opcional da transação
        public string? Descricao { get; set; }

        // Data em que a transação ocorreu
        public DateTime Data { get; set; }
    }
}
