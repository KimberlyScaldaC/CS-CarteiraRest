
using CS_CarteiraRest.Model;
using Microsoft.EntityFrameworkCore;

namespace CS_CarteiraRest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria o construtor da aplicação web
            var builder = WebApplication.CreateBuilder(args);

            // Adiciona os serviços de controllers ao container de injeção de dependência
            builder.Services.AddControllers();

            // Adiciona suporte para endpoints mínimos e exploração via Swagger
            builder.Services.AddEndpointsApiExplorer();

            // Configura o banco de dados em memória (ideal para testes e protótipos)
            builder.Services.AddDbContext<CarteiraContext>(opt =>
                opt.UseInMemoryDatabase("CarteiraBD"));

            // Adiciona o gerador do Swagger para documentação automática da API
            builder.Services.AddSwaggerGen();

            // Constrói o app com as configurações definidas
            var app = builder.Build();

            // Configura o pipeline HTTP da aplicação
            if (app.Environment.IsDevelopment())
            {
                // Ativa Swagger apenas em ambiente de desenvolvimento
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware para autorização (não há autenticação configurada, mas está pronto)
            app.UseAuthorization();

            // Mapeia os controllers da API (como o CarteirasController)
            app.MapControllers();

            // Executa a aplicação
            app.Run();
        }
    }
}
