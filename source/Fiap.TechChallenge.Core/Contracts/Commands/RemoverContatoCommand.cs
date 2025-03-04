using Fiap.TechChallenge.Core.Contracts.Commands.CommandResults;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Core.Contracts.Commands
{
    public class RemoverContatoCommand : ICommand<RemoverContatoCommandResult>
    {
        public string Id { get; set; }
    }
}
