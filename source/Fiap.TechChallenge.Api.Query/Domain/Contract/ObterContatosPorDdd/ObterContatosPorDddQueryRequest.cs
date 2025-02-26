using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatosPorDdd;

public class ObterContatosPorDddQueryRequest : IQueryRequest<ObterContatosPorDddQueryResult>
{
    public int Ddd { get; set; }
}