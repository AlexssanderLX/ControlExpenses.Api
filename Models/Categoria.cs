using ControlExpenses.Api.Enums;

namespace ControlExpenses.Api.Models
{
    public class Categoria
    {
        //Propiedades sempre com private set para que sejam alteradas somente por métodos.

        public int Id { get; private set; }
        public string Descricao { get; private set; }
        // Enum para estado fixo da finalidade da categoria.
        public FinalidadeCategoria Finalidade { get; private set; }

        // Construtor validando a descrição
        public Categoria(string descricao, FinalidadeCategoria finalidade)
        {
            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException("Descrição é obrigatória", nameof(descricao));
            }

            Descricao = descricao;
            Finalidade = finalidade;
        }
        //Construtor exclusivo para EF core
        private Categoria() { }
    }
}
