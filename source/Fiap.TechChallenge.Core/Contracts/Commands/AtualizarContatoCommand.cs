using Fiap.TechChallenge.Core.Contracts.Commands.CommandResults;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using System.Text.Json.Serialization;

namespace Fiap.TechChallenge.Core.Contracts.Commands
{
    public class AtualizarContatoCommand : ICommand<AtualizarContatoCommandResult>
    {
        [JsonIgnore] public string? Id { get; set; }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int DDD { get; set; }
    }
}
