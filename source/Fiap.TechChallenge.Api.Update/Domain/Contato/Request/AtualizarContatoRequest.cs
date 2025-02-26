namespace Fiap.TechChallenge.Api.Update.Domain.Contato.Request;

public class AtualizarContatoRequest
{
    public AtualizarContatoRequest(Guid id, string nome, string telefone, string email, int ddd)
    {
        Id = id;
        Nome = nome;
        Telefone = telefone;
        Email = email;
        DDD = ddd;
    }

    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int DDD { get; set; }
}