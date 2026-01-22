using ControlExpenses.Api.Dtos.TransacaoDtos;
using ControlExpenses.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControlExpenses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly TransacaoService _service;

        public TransacoesController(TransacaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] TransacaoCreateDto dto)
        {
            try
            {
                var result = await _service.CriarAsync(dto);
                return Ok(result);
            }
            catch (BusinessExceptions ex)
            {
                // Erro de regra de negócio -> 400
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var result = await _service.ListarAsync();
            return Ok(result);
        }
    }
}

