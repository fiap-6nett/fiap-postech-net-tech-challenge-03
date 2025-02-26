namespace Fiap.TC03.Api.Consulta.Domain.Contato.Request;

public class ObterContatosPorDddRequest
{
    public ObterContatosPorDddRequest(int ddd)
    {
        Ddd = ddd;
    }

    public int Ddd { get; set; }
}