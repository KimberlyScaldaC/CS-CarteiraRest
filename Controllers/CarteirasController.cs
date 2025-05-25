using CS_CarteiraRest.Model;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS_CarteiraRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarteirasController : ControllerBase
    {
        private readonly CarteiraContext _context;

        public CarteirasController(CarteiraContext context)
        {
            _context = context;
        }

        // Classe auxiliar para transferência de valores entre carteiras
        public class TransferenciaRequest
        {
            public int IdOrigem { get; set; }
            public int IdDestino { get; set; }
            public decimal Valor { get; set; }
        }

        // Retorna todas as carteiras cadastradas.
        // GET: api/Carteiras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carteira>>> GetCarteiraItens()
        {
            //GetCarteiraCoinItens
            if (_context.CarteiraItens == null)
            {
                return NotFound();
            }

            return await _context.CarteiraItens.ToListAsync();
        }

        // Retorna uma carteira pelo seu ID.
        // GET: api/Carteiras/5
        [HttpGet("Exibir/{id}")]
        public async Task<ActionResult<Carteira>> GetCarteira(int id)
        {
            var carteira = await _context.CarteiraItens.FindAsync(id);

            if (carteira == null)
            {
                return NotFound();
            }

            return carteira;
        }

        // Ajusta o saldo de uma carteira (pode ser positivo ou negativo).
        // PUT: api/Carteiras/5/saldo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("AjustaSaldo/{id}/{saldo}")]
        public async Task<IActionResult> PutCarteira(int id, Decimal saldo)
        {
            var carteira = await _context.CarteiraItens.FindAsync(id);

            if (carteira == null)
            {
                return NotFound();
            }

            decimal diferenca = saldo + carteira.Saldo;
            string tipo = diferenca > 0 ? "Ajuste de Adição" : "Ajuste de Remoção";

            carteira.Saldo += saldo;

            if (carteira.Saldo < 0)
            {
                return BadRequest("Saldo insuficiente para esta operação.");
            }

            // Registra transação
            _context.Transacoes.Add(new Transacao
            {
                IdCarteira = id,
                Valor = saldo,
                Tipo = tipo,
                Descricao = $"Ajuste de saldo para {carteira.Saldo}",
                Data = DateTime.UtcNow
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarteiraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Cria uma nova carteira.
        // POST: api/Carteiras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Carteira>> PostCarteira(Carteira carteira)
        {
            //PostCarteiraCoin
            if (_context.CarteiraItens == null)
            {
                return Problem("Entity set 'CarteiraCoinContext.CarteiraCoinItens' is null.");
            }

            _context.CarteiraItens.Add(carteira);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarteira", new { id = carteira.Id }, carteira);
        }

        // Retorna o saldo atual de uma carteira.
        // GET: api/Carteiras/ConsultarSaldo/5
        [HttpGet("ConsultarSaldo/{id}")]
        public async Task<ActionResult<decimal>> ConsultarSaldo(int id)
        {
            var carteira = await _context.CarteiraItens.FindAsync(id);

            if (carteira == null)
            {
                return NotFound("Carteira não encontrada.");
            }

            return Ok(carteira.Saldo);
        }


        // Realiza uma transferência de saldo entre duas carteiras.
        // POST: api/Carteiras/transferir
        [HttpPost("Transferir")]
        public async Task<IActionResult> Transferir(TransferenciaRequest request)
        {
            if (request.Valor <= 0)
            {
                return BadRequest("O valor da transferência deve ser maior que zero.");
            }

            var origem = await _context.CarteiraItens.FindAsync(request.IdOrigem);
            var destino = await _context.CarteiraItens.FindAsync(request.IdDestino);

            if (origem == null || destino == null)
            {
                return NotFound("Carteira de origem ou destino não encontrada.");
            }

            if (origem.Saldo < request.Valor)
            {
                return BadRequest("Saldo insuficiente na carteira de origem.");
            }

            // Realiza a transferência
            origem.Saldo -= request.Valor;
            destino.Saldo += request.Valor;

            // Após debitar da origem
            _context.Transacoes.Add(new Transacao
            {
                IdCarteira = origem.Id,
                Valor = -request.Valor,
                Tipo = "transferencia",
                Descricao = $"Transferido para carteira {request.IdDestino}",
                Data = DateTime.UtcNow
            });

            // Após creditar no destino
            _context.Transacoes.Add(new Transacao
            {
                IdCarteira = destino.Id,
                Valor = request.Valor,
                Tipo = "transferencia",
                Descricao = $"Recebido de carteira {request.IdOrigem}",
                Data = DateTime.UtcNow
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao salvar a transferência: " + ex.Message);
            }

            return Ok(new
            {
                Mensagem = "Transferência realizada com sucesso.",
            });
        }

        // Remove uma carteira pelo ID.
        // DELETE: api/Carteiras/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCarteira(int id)
        {
            //DeleteCarteiraCoin
            if (_context.CarteiraItens == null)
            {
                return NotFound();
            }

            var carteira = await _context.CarteiraItens.FindAsync(id);
            if (carteira == null)
            {
                return NotFound();
            }

            _context.CarteiraItens.Remove(carteira);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método auxiliar para verificar existência de carteira
        private bool CarteiraExists(int id)
        {
            return _context.CarteiraItens.Any(e => e.Id == id);
        }
    }
}
