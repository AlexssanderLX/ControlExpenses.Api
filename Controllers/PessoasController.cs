// Controllers/PessoasController.cs
using ControlExpenses.Api.Data;
using ControlExpenses.Api.Dtos.PessoaDtos;
using ControlExpenses.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlExpenses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PessoasController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /api/pessoas
        [HttpPost]
        public async Task<ActionResult<PessoaResponseDto>> Create([FromBody] PessoaCreateDto dto)
        {
            try
            {
                var pessoa = new Pessoa(dto.Nome, dto.Idade);

                _context.Pessoas.Add(pessoa);
                await _context.SaveChangesAsync();

                var response = new PessoaResponseDto
                {
                    Id = pessoa.Id,
                    Nome = pessoa.Nome,
                    Idade = pessoa.Idade
                };

                return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // GET: /api/pessoas
        [HttpGet]
        public async Task<ActionResult<List<PessoaResponseDto>>> GetAll()
        {
            var pessoas = await _context.Pessoas
                .AsNoTracking()
                .Select(p => new PessoaResponseDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Idade = p.Idade
                })
                .ToListAsync();

            return Ok(pessoas);
        }

        // GET: /api/pessoas/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PessoaResponseDto>> GetById([FromRoute] int id)
        {
            var pessoa = await _context.Pessoas
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa is null)
                return NotFound();

            var response = new PessoaResponseDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade
            };

            return Ok(response);
        }

        // DELETE: /api/pessoas/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var pessoa = await _context.Pessoas.FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa is null)
                return NotFound();

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            // Cascade delete já remove Transacoes da pessoa (configurado no AppDbContext)
            return NoContent();
        }
    }
}