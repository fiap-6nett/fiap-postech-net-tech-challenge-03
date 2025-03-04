using Fiap.TechChallenge.Core.Contracts.Commands;
using Fiap.TechChallenge.Core.Contracts.Commands.CommandResults;
using Fiap.TechChallenge.Core.Contracts.Requests;
using Fiap.TechChallenge.Core.Services;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Core.Handlers.CommandHandlers
{
    public class CriarContatoCommandHandler : ICommandHandler<CriarContatoCommand, CriarContatoCommandResult>
    {
        private readonly ILogger<CriarContatoCommandHandler> _logger;
        private readonly IContatoService _service;

        public CriarContatoCommandHandler(IContatoService service, ILogger<CriarContatoCommandHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<CriarContatoCommandResult> Handle(CriarContatoCommand commandRequest)
        {
            var result = await _service.CriarContatoQueueAsync(new CriarContatoRequest(commandRequest.Nome, commandRequest.Telefone, commandRequest.Email, commandRequest.DDD));
            return new CriarContatoCommandResult
            {
                CorrelationId = result.Contato.Id,
                Message = "Solicitação recebida e será processada."
            };
        }

        public Task OnError(Exception exception, CriarContatoCommand commandRequest)
        {
            throw exception;
        }
    }
}
