namespace Fiap.TechChallenge.Api.Delete.Domain.Contato.Request;

public class RemoverContatoRequest
{
    public RemoverContatoRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}