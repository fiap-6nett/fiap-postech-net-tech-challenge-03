using Fiap.TechChallenge.Core.Contracts.Queries.QueryResults;
using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TechChallenge.Core.Contracts.Queries
{
    public class ObterContatoPorIdQueryRequest : IQueryRequest<ObterContatoPorIdQueryResult>
    {
        public string Id { get; set; }
    }
}
