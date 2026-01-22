namespace ControlExpenses.Api.Services
{
    public class BusinessExceptions : Exception
    {
        public BusinessExceptions(string message) : base(message) { }
    }
}
