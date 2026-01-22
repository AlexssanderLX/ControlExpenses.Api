// Controllers/RelatoriosController.cs
using ControlExpenses.Api.Data;
using ControlExpenses.Api.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlExpenses.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatoriosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/relatorios/totais-por-pessoa
        // Retorna totais de receitas/despesas/saldo por pessoa e o total geral no final.
        [HttpGet("totais-por-pessoa")]
        public async Task<ActionResult<RelatorioTotaisPorPessoaResponseDto>> TotaisPorPessoa()
        {
            // Carrega pessoas e transações (sem Tracking)
            var pessoas = await _context.Pessoas
                .AsNoTracking()
                .Select(p => new { p.Id, p.Nome, p.Idade })
                .ToListAsync();

            var transacoes = await _context.Transacoes
                .AsNoTracking()
                .Select(t => new { t.PessoaId, t.Tipo, t.Valor })
                .ToListAsync();

            // Agrupa transações por pessoa
            var porPessoa = transacoes
                .GroupBy(t => t.PessoaId)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Receitas = g.Where(x => x.Tipo == TipoTransacao.Receita).Sum(x => x.Valor),
                        Despesas = g.Where(x => x.Tipo == TipoTransacao.Despesa).Sum(x => x.Valor)
                    }
                );

            var lista = pessoas.Select(p =>
            {
                porPessoa.TryGetValue(p.Id, out var tot);

                var receitas = tot?.Receitas ?? 0m;
                var despesas = tot?.Despesas ?? 0m;

                return new RelatorioPessoaTotaisDto
                {
                    PessoaId = p.Id,
                    Nome = p.Nome,
                    Idade = p.Idade,
                    TotalReceitas = receitas,
                    TotalDespesas = despesas,
                    Saldo = receitas - despesas
                };
            }).ToList();

            var totalGeralReceitas = lista.Sum(x => x.TotalReceitas);
            var totalGeralDespesas = lista.Sum(x => x.TotalDespesas);

            var response = new RelatorioTotaisPorPessoaResponseDto
            {
                Pessoas = lista,
                TotalGeral = new RelatorioTotaisGeraisDto
                {
                    TotalReceitas = totalGeralReceitas,
                    TotalDespesas = totalGeralDespesas,
                    Saldo = totalGeralReceitas - totalGeralDespesas
                }
            };

            return Ok(response);
        }

        // DTOs do relatório (pra não criar arquivos agora e já compilar)
        public class RelatorioTotaisPorPessoaResponseDto
        {
            public List<RelatorioPessoaTotaisDto> Pessoas { get; set; } = new();
            public RelatorioTotaisGeraisDto TotalGeral { get; set; } = new();
        }

        public class RelatorioPessoaTotaisDto
        {
            public int PessoaId { get; set; }
            public string Nome { get; set; } = string.Empty;
            public int Idade { get; set; }

            public decimal TotalReceitas { get; set; }
            public decimal TotalDespesas { get; set; }
            public decimal Saldo { get; set; }
        }

        public class RelatorioTotaisGeraisDto
        {
            public decimal TotalReceitas { get; set; }
            public decimal TotalDespesas { get; set; }
            public decimal Saldo { get; set; }
        }
    }
}
