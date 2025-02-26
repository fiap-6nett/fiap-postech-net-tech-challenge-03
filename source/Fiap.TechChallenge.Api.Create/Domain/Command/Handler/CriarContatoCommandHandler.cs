﻿using Fiap.TechChallenge.Api.Create.Domain.Contato;
using Fiap.TechChallenge.Api.Create.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Create.Domain.Contract.CriarContato;
using Fiap.TechChallenge.Foundation.Core.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Api.Create.Domain.Command.Handler;

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
            CorrelationId = result.Contato.Id,
            Message = "Solicitação recebida e será processada."
        };
    }

    public Task OnError(Exception exception, CriarContatoCommand commandRequest)
    {
        throw exception;
    }
}