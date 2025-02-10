using Fiap.TC03.Api.Cadastro.Domain.Contato;
using Fiap.TC03.Api.Cadastro.Domain.Contato.Request;
using Fiap.TC03.Api.Cadastro.Domain.Contract.CriarContato;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Fiap.TC03.Api.Cadastro.Domain.Command.Handler.v1;

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
        var result = await _service.CriarContatoAsync(new CriarContatoRequest(commandRequest.Nome, commandRequest.Telefone, commandRequest.Email, commandRequest.DDD));
        return new CriarContatoCommandResult
        {
            CorrelationId = result.Contato.Id
        };
    }

    public Task OnError(Exception exception, CriarContatoCommand commandRequest)
    {
        throw exception;
    }
}