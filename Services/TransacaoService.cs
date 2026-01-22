using ControlExpenses.Api.Data;
using ControlExpenses.Api.Dtos.TransacaoDtos;
using ControlExpenses.Api.Enums;
using ControlExpenses.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlExpenses.Api.Services
{
    // Service responsável por conter regras de negócio relacionadas à Transacao
    public class TransacaoService
    {
        private readonly AppDbContext _context;

        public TransacaoService(AppDbContext context)
        {
            _context = context;
        }

        // Cria uma transação aplicando as regras do enunciado.
        public async Task<TransacaoResponseDto> CriarAsync(TransacaoCreateDto dto)
        {
            // Garantir existência da pessoa
            var pessoa = await _context.Pessoas
                .FirstOrDefaultAsync(p => p.Id == dto.PessoaId);

            if (pessoa is null)
                throw new ArgumentException("Pessoa não encontrada.");

            // Garantir existência da categoria
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == dto.CategoriaId);

            if (categoria is null)
                throw new ArgumentException("Categoria não encontrada.");

            // Menor de idade só pode registrar despesas
            if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
                throw new ArgumentException("Pessoa menor de idade só pode registrar despesas.");

            // Categoria deve ser compatível com o tipo da transação
            if (dto.Tipo == TipoTransacao.Despesa && categoria.Finalidade == FinalidadeCategoria.Receita)
                throw new ArgumentException("Categoria com finalidade 'Receita' não pode ser usada em uma despesa.");

            // Se transação é Receita, categoria não pode ser Despesa
            if (dto.Tipo == TipoTransacao.Receita && categoria.Finalidade == FinalidadeCategoria.Despesa)
                throw new ArgumentException("Categoria com finalidade 'Despesa' não pode ser usada em uma receita.");

            // Criação da entidade via construtor (mantém encapsulamento com private set)
            var transacao = new Transacao(
                dto.Descricao,
                dto.Valor,
                dto.Tipo,
                dto.PessoaId,
                dto.CategoriaId
            );

            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();

            // Retorno em Dto
            return new TransacaoResponseDto
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                PessoaId = transacao.PessoaId,
                CategoriaId = transacao.CategoriaId
            };
        }

        // Lista de Transações
        public async Task<List<TransacaoResponseDto>> ListarAsync()
        {
            return await _context.Transacoes
                .AsNoTracking()
                .Select(t => new TransacaoResponseDto
                {
                    Id = t.Id,
                    Descricao = t.Descricao,
                    Valor = t.Valor,
                    Tipo = t.Tipo,
                    PessoaId = t.PessoaId,
                    CategoriaId = t.CategoriaId
                })
                .ToListAsync();
        }
    }
}
