using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TC03.Api.Atualizacao.Domain.Contract;

public class AtualizarContatoCommandResult : CommandResult
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int DDD { get; set; }
}