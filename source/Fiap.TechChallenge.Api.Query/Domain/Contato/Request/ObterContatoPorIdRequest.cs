namespace Fiap.TechChallenge.Api.Query.Domain.Contato.Request;

public class ObterContatoPorIdRequest
{
    public ObterContatoPorIdRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}