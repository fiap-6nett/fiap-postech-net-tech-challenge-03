namespace Fiap.TechChallenge.Core.Contracts.Queries.QueryResults
{
    public sealed class ObterContatosPorDddQueryResult
    {
        /// <summary>
        ///     Lista de contatos retornados pela consulta.
        /// </summary>
        public List<ContatoQueryResult> Contatos { get; set; }
    }
}
