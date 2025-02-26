namespace Fiap.TechChallenge.Api.Query.Domain.Contato.Request;

public class ObterContatosPorDddRequest
{
    public ObterContatosPorDddRequest(int ddd)
    {
        Ddd = ddd;
    }

    public int Ddd { get; set; }
}