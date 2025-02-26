using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TC03.Api.Atualizacao.Domain.Contract;

public class AtualizarContatoCommandResult : CommandResult
{
    public Guid CorrelationId { get; set; }
    public DateTime DataRecebimento { get; set; } = DateTime.UtcNow;
}