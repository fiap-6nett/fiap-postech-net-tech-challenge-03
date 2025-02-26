using Fiap.TechChallenge.Api.Delete.Domain.Contato;
using Fiap.TechChallenge.Api.Delete.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Delete.Domain.Contract;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Api.Delete.Domain.Command.Handler;

public class RemoverContatoCommandHandler : ICommandHandler<RemoverContatoCommand, RemoverContatoCommandResult>
{
    private readonly ILogger<RemoverContatoCommandHandler> _logger;
    private readonly IContatoService _service;

    public RemoverContatoCommandHandler(ILogger<RemoverContatoCommandHandler> logger, IContatoService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<RemoverContatoCommandResult> Handle(RemoverContatoCommand commandRequest)
    {
        var result = await _service.RemoverContatoAsync(new RemoverContatoRequest(Guid.Parse(commandRequest.Id)));
        return new RemoverContatoCommandResult
        {
            CorrelationId = Guid.Parse(commandRequest.Id),
            Message = "Solicitação recebida e será processada."
        };
    }

    public Task OnError(Exception exception, RemoverContatoCommand commandRequest)
    {
        throw exception;
    }
}