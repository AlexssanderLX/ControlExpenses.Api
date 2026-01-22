namespace ControlExpenses.Api.Models
{
    public class Pessoa
    {
        //Propiedades sempre com private set para que sejam alteradas somente por métodos.
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public int Idade { get; private set; }
        public List<Transacao> Transacoes { get; private set; }

        public Pessoa(string nome, int idade)
        {
            //Condicional que não permite que o usuário tenha um nome nulo ou com caracteres inválidos.
            if (string.IsNullOrWhiteSpace(nome))
            {

                throw new ArgumentNullException(nameof(nome), "É obrigatório um nome válido!");
            }
            //Condicional que impede que idade receba um valor igual ou abaixo de zero.
            if (idade <= 0)
            {
                throw new ArgumentOutOfRangeException("Digite uma idade válida!");
            }
            Nome = nome;
            Idade = idade;
            Transacoes = new List<Transacao>();
        }
        //Construtor vazio para o EF Core
        private Pessoa() { }
    }
}
