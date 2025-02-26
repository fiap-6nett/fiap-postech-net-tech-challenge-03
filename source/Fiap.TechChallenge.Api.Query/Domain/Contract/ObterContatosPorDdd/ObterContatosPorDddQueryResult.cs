namespace Fiap.TechChallenge.Api.Query.Domain.Contract.ObterContatosPorDdd;

public sealed class ObterContatosPorDddQueryResult
{
    /// <summary>
    ///     Lista de contatos retornados pela consulta.
    /// </summary>
    public List<ContatoQueryResult> Contatos { get; set; }
}