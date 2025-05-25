using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CS_CarteiraRest.Model;

namespace CS_CarteiraRest.Controllers
{
    // Define a rota base para este controller: api/Transacaos
    [Route("api/[controller]")]
    [ApiController]
    public class TransacaosController : ControllerBase
    {
        // Contexto do EF para acesso ao banco de dados
        private readonly CarteiraContext _context;

        // Injeção de dependência do contexto via construtor
        public TransacaosController(CarteiraContext context)
        {
            _context = context;
        }


        // Recupera todas as transações de uma carteira específica
        // GET: api/Transacaos/{Idcarteira}
        [HttpGet("{Idcarteira}")]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacoesPorCarteira(int Idcarteira)
        {
            // Filtra transações pela carteira e ordena por data decrescente
            var transacoes = await _context.Transacoes
                                           .Where(t => t.IdCarteira == Idcarteira)
                                           .OrderByDescending(t => t.Data)
                                           .ToListAsync();

            return transacoes;
        }

        
        // Cria uma nova transação para uma carteira
        // POST: api/Transacaos
        [HttpPost]
        public async Task<ActionResult<Transacao>> PostTransacao(Transacao transacao)
        {
            // Garante que a data seja registrada em UTC no momento da criação
            transacao.Data = DateTime.UtcNow;

            // Adiciona a transação ao contexto
            _context.Transacoes.Add(transacao);
            // Persiste as mudanças no banco de dados
            await _context.SaveChangesAsync();

            // Retorna 201 Created com cabeçalho Location apontando para GetTransacoesPorCarteira
            return CreatedAtAction(
                nameof(GetTransacoesPorCarteira),
                new { Idcarteira = transacao.IdCarteira },
                transacao
            );
        }

        
       // Verifica se uma transação existe no banco de dados.
       private bool TransacaoExists(int id)
        {
            // Usa Any para verificar existência sem carregar a entidade completa
            return _context.Transacoes.Any(e => e.Id == id);
        }
    }
}
