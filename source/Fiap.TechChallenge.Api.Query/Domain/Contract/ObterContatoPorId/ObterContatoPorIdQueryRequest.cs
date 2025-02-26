using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TechChallenge.Api.Query.Domain.Contract.ObterContatoPorId;

public class ObterContatoPorIdQueryRequest : IQueryRequest<ObterContatoPorIdQueryResult>
{
    public string Id { get; set; }
}