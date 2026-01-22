using ControlExpenses.Api.Enums;

namespace ControlExpenses.Api.Models
{
    public class Transacao
    {
        // Propriedades sempre com private set para que sejam alteradas somente por métodos.
        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public decimal Valor { get; private set; }

        // Enum para estado fixo do Tipo da transação.
        public TipoTransacao Tipo { get; private set; }

        // Propriedade para identificação do EF sobre o objeto Pessoa.
        public int PessoaId { get; private set; }
        public Pessoa? Pessoa { get; private set; }

        // Propriedade para identificação do EF sobre o objeto Categoria.
        public int CategoriaId { get; private set; }
        public Categoria? Categoria { get; private set; }

        // Construtor com algumas exceções básicas para integridade dos dados
        // OBS: regras de negócio mais complexas ficam na camada de Service
        public Transacao(
            string descricao,
            decimal valor,
            TipoTransacao tipo,
            int pessoaId,
            int categoriaId)
        {
            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException("Descrição é obrigatória.", nameof(descricao));
            }

            if (valor <= 0)
            {
                throw new ArgumentException("O valor da transação deve ser maior que zero.", nameof(valor));
            }

            Descricao = descricao.Trim();
            Valor = valor;
            Tipo = tipo;
            PessoaId = pessoaId;
            CategoriaId = categoriaId;
        }

        // Construtor vazio para uso do EF Core
        private Transacao() { }
    }
}
