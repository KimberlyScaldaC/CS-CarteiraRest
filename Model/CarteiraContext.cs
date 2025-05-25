using Microsoft.EntityFrameworkCore;

namespace CS_CarteiraRest.Model
{
    // Classe de contexto do Entity Framework, responsável por gerenciar a conexão com o banco de dados
    public class CarteiraContext : DbContext
    {
        // Construtor que recebe as opções de configuração do contexto
        public CarteiraContext(DbContextOptions<CarteiraContext> options)
                     : base(options)
        {
            // As opções (como o tipo de banco e a string de conexão) são passadas para a classe base DbContext
        }

        // Representa a tabela de Carteiras no banco de dados
        public DbSet<Carteira> CarteiraItens { get; set; } = null!;

        // Representa a tabela de Transações no banco de dados
        public DbSet<Transacao> Transacoes { get; set; } = null!;
    }
}
