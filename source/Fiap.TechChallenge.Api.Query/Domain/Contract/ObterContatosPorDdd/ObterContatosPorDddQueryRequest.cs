using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TechChallenge.Api.Query.Domain.Contract.ObterContatosPorDdd;

public class ObterContatosPorDddQueryRequest : IQueryRequest<ObterContatosPorDddQueryResult>
{
    public int Ddd { get; set; }
}