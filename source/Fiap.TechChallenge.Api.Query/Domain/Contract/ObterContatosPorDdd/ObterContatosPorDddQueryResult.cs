namespace Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatosPorDdd;

public sealed class ObterContatosPorDddQueryResult
{
    /// <summary>
    ///     Lista de contatos retornados pela consulta.
    /// </summary>
    public List<ContatoQueryResult> Contatos { get; set; }
}