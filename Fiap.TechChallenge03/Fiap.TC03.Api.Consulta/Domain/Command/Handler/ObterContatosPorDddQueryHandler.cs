﻿using Fiap.TC03.Api.Consulta.Domain.Contato;
using Fiap.TC03.Api.Consulta.Domain.Contato.Request;
using Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatosPorDdd;
using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;
using Microsoft.Extensions.Logging;

namespace Fiap.TC03.Api.Consulta.Domain.Command.Handler;

public class ObterContatosPorDddQueryHandler : IQueryHandler<ObterContatosPorDddQueryRequest, ObterContatosPorDddQueryResult>
{
    private readonly ILogger<ObterContatosPorDddQueryHandler> _logger;
    private readonly IContatoService _service;

    public ObterContatosPorDddQueryHandler(ILogger<ObterContatosPorDddQueryHandler> logger, IContatoService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<ObterContatosPorDddQueryResult> Handle(ObterContatosPorDddQueryRequest queryRequest)
    {
        var porDddResult = await _service.ObterContatosPorDddAsync(new ObterContatosPorDddRequest(queryRequest.Ddd));
        var result = porDddResult.Contatos.Select(contato => new ContatoQueryResult(contato.Id, contato.Nome, contato.Telefone, contato.Email, contato.DDD)).ToList();

        return new ObterContatosPorDddQueryResult
        {
            Contatos = result
        };
    }
}