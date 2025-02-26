using Fiap.TC03.Api.Consulta.Domain.Contato;
using Fiap.TC03.Api.Consulta.Domain.Contato.Request;
using Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatoPorId;
using Fiap.TechChallenge.Foundation.Core.Messaging.Queries;
using Microsoft.Extensions.Logging;

namespace Fiap.TC03.Api.Consulta.Domain.Command.Handler;

public class ObterContatoPorIdQueryHandler : IQueryHandler<ObterContatoPorIdQueryRequest, ObterContatoPorIdQueryResult>
{
    private readonly ILogger<ObterContatoPorIdQueryHandler> _logger;
    private readonly IContatoService _service;

    public ObterContatoPorIdQueryHandler(ILogger<ObterContatoPorIdQueryHandler> logger, IContatoService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<ObterContatoPorIdQueryResult> Handle(ObterContatoPorIdQueryRequest queryRequest)
    {
        var result = await _service.ObterContatoPorIdAsync(new ObterContatoPorIdRequest(Guid.Parse(queryRequest.Id)));
        return new ObterContatoPorIdQueryResult
        {
            Id = result.Contato.Id,
            Nome = result.Contato.Nome,
            Telefone = result.Contato.Telefone,
            Email = result.Contato.Email,
            DDD = result.Contato.DDD
        };
    }
}