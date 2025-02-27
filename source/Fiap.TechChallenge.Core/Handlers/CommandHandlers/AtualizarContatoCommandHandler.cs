using Fiap.TechChallenge.Core.Contracts.Commands;
using Fiap.TechChallenge.Core.Contracts.Commands.CommandResults;
using Fiap.TechChallenge.Core.Contracts.Requests;
using Fiap.TechChallenge.Core.Services;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Core.Handlers.CommandHandlers
{
    public class AtualizarContatoCommandHandler : ICommandHandler<AtualizarContatoCommand, AtualizarContatoCommandResult>
    {
        private readonly ILogger<AtualizarContatoCommandHandler> _logger;
        private readonly IContatoService _service;

        public AtualizarContatoCommandHandler(ILogger<AtualizarContatoCommandHandler> logger, IContatoService service)
        {
            _logger = logger;
            _service = service;
        }


        public async Task<AtualizarContatoCommandResult> Handle(AtualizarContatoCommand commandRequest)
        {
            var result = await _service.AtualizarContatoAsync(new AtualizarContatoRequest(Guid.Parse(commandRequest.Id), commandRequest.Nome, commandRequest.Telefone, commandRequest.Email, commandRequest.DDD));
            return new AtualizarContatoCommandResult
            {
                CorrelationId = result.Contato.Id,
                Message = "Solicitação recebida e será processada."
            };
        }

        public Task OnError(Exception exception, AtualizarContatoCommand commandRequest)
        {
            throw exception;
        }
    }
}
