using Fiap.TC03.Api.Consulta.Domain.DataBaseContext;

namespace Fiap.TC03.Api.Consulta.Domain.Result;

public class ObterContatosPorDddResult
{
    public ObterContatosPorDddResult(List<ContatoEntity> contatos)
    {
        Contatos = contatos;
    }

    public List<ContatoEntity> Contatos { get; set; }
}