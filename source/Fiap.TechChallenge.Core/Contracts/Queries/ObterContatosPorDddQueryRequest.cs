using Fiap.TechChallenge.Core.Contracts.Queries.QueryResults;
using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

namespace Fiap.TechChallenge.Core.Contracts.Queries
{
    public class ObterContatosPorDddQueryRequest : IQueryRequest<ObterContatosPorDddQueryResult>
    {
        public int Ddd { get; set; }
    }
}
