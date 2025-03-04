namespace Fiap.TechChallenge.Core.Contracts.Requests
{
    public class CriarContatoRequest
    {
        public CriarContatoRequest(string nome, string telefone, string email, int ddd)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            DDD = ddd;
        }

        public string Nome { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public int DDD { get; private set; }
    }
}
