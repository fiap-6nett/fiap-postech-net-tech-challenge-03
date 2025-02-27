namespace Fiap.TechChallenge.Core.Contracts.Queries.QueryResults
{
    public sealed class ObterContatoPorIdQueryResult
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int DDD { get; set; }
    }
}
