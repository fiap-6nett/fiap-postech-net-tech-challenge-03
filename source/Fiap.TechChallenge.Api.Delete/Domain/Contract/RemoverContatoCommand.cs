using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TC03.Api.Exclusao.Domain.Contract;
public class RemoverContatoCommand : ICommand<RemoverContatoCommandResult>
{
    public string Id { get; set; }
}