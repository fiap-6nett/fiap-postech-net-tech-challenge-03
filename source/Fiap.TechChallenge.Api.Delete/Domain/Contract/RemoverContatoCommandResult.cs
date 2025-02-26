using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Api.Delete.Domain.Contract;
public class RemoverContatoCommandResult : CommandResult
{
    public Guid CorrelationId { get; set; }
    public DateTime DataRecebimento { get; set; } = DateTime.UtcNow;
}