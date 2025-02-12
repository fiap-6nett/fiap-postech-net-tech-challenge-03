using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatoPorId;

public class ObterContatoPorIdQueryRequest : IQueryRequest<ObterContatoPorIdQueryResult>
{
    public string Id { get; set; }
}