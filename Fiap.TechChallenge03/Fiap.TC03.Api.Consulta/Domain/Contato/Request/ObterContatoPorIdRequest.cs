namespace Fiap.TC03.Api.Consulta.Domain.Contato.Request;

public class ObterContatoPorIdRequest
{
    public ObterContatoPorIdRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}