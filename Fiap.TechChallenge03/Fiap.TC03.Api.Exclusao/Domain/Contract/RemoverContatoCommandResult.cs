using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TC03.Api.Exclusao.Domain.Contract;
public class RemoverContatoCommandResult : CommandResult
{
    public bool Sucesso { get; set; }
}