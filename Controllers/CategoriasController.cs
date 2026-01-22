// Controllers/CategoriasController.cs
using ControlExpenses.Api.Data;
using ControlExpenses.Api.Dtos.CategoriaDtos;
using ControlExpenses.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlExpenses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /api/categorias
        [HttpPost]
        public async Task<ActionResult<CategoriaResponseDto>> Create([FromBody] CategoriaCreateDto dto)
        {
            try
            {
                var categoria = new Categoria(dto.Descricao, dto.Finalidade);

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                var response = new CategoriaResponseDto
                {
                    Id = categoria.Id,
                    Descricao = categoria.Descricao,
                    Finalidade = categoria.Finalidade
                };

                return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // GET: /api/categorias
        [HttpGet]
        public async Task<ActionResult<List<CategoriaResponseDto>>> GetAll()
        {
            var categorias = await _context.Categorias
                .AsNoTracking()
                .Select(c => new CategoriaResponseDto
                {
                    Id = c.Id,
                    Descricao = c.Descricao,
                    Finalidade = c.Finalidade
                })
                .ToListAsync();

            return Ok(categorias);
        }

        // GET: /api/categorias/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaResponseDto>> GetById([FromRoute] int id)
        {
            var categoria = await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria is null)
                return NotFound();

            var response = new CategoriaResponseDto
            {
                Id = categoria.Id,
                Descricao = categoria.Descricao,
                Finalidade = categoria.Finalidade
            };

            return Ok(response);
        }
    }
}
