using ControlExpenses.Api.Enums;

namespace ControlExpenses.Api.Dtos.CategoriaDtos
{
    public class CategoriaCreateDto
    {
        public string Descricao { get; set; } = string.Empty;
        public FinalidadeCategoria Finalidade { get; set; }
    }
}
