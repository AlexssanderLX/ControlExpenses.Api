using ControlExpenses.Api.Enums;

namespace ControlExpenses.Api.Dtos.CategoriaDtos
{
    public class CategoriaResponseDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public FinalidadeCategoria Finalidade { get; set; }

    }
}
