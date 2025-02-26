using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Api.Create.Domain.Contract.CriarContato;

public class CriarContatoCommand : ICommand<CriarContatoCommandResult>
{
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int DDD { get; set; }
}