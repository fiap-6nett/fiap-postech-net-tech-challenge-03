using System.Text.Json.Serialization;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;

namespace Fiap.TechChallenge.Api.Update.Domain.Contract;

public class AtualizarContatoCommand : ICommand<AtualizarContatoCommandResult>
{
    [JsonIgnore] public string? Id { get; set; }

    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public int DDD { get; set; }
}