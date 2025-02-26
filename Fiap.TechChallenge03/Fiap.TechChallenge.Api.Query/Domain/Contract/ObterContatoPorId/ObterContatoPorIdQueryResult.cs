namespace Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatoPorId;

public sealed class ObterContatoPorIdQueryResult
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int DDD { get; set; }
}