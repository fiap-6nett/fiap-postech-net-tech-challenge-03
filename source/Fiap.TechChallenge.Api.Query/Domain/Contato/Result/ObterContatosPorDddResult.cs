using Fiap.TechChallenge.Api.Query.Domain.DataBaseContext;

namespace Fiap.TechChallenge.Api.Query.Domain.Result;

public class ObterContatosPorDddResult
{
    public ObterContatosPorDddResult(List<ContatoEntity> contatos)
    {
        Contatos = contatos;
    }

    public List<ContatoEntity> Contatos { get; set; }
}