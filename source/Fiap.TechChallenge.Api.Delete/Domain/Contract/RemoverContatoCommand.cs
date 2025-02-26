using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Api.Delete.Domain.Contract;
public class RemoverContatoCommand : ICommand<RemoverContatoCommandResult>
{
    public string Id { get; set; }
}