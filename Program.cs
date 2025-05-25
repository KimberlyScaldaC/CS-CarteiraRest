
using CS_CarteiraRest.Model;
using Microsoft.EntityFrameworkCore;

namespace CS_CarteiraRest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria o construtor da aplica��o web
            var builder = WebApplication.CreateBuilder(args);

            // Adiciona os servi�os de controllers ao container de inje��o de depend�ncia
            builder.Services.AddControllers();

            // Adiciona suporte para endpoints m�nimos e explora��o via Swagger
            builder.Services.AddEndpointsApiExplorer();

            // Configura o banco de dados em mem�ria (ideal para testes e prot�tipos)
            builder.Services.AddDbContext<CarteiraContext>(opt =>
                opt.UseInMemoryDatabase("CarteiraBD"));

            // Adiciona o gerador do Swagger para documenta��o autom�tica da API
            builder.Services.AddSwaggerGen();

            // Constr�i o app com as configura��es definidas
            var app = builder.Build();

            // Configura o pipeline HTTP da aplica��o
            if (app.Environment.IsDevelopment())
            {
                // Ativa Swagger apenas em ambiente de desenvolvimento
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware para autoriza��o (n�o h� autentica��o configurada, mas est� pronto)
            app.UseAuthorization();

            // Mapeia os controllers da API (como o CarteirasController)
            app.MapControllers();

            // Executa a aplica��o
            app.Run();
        }
    }
}
