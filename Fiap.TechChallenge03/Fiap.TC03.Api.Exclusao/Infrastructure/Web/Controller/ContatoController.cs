using Fiap.TC03.Api.Exclusao.Domain.Command.Handler;
using Fiap.TC03.Api.Exclusao.Domain.Contract;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace Fiap.TC03.Api.Exclusao.Infrastructure.Web.Controller;

public class ContatoController
{
    private const string DescriptionRemocao =
        "Remove um contato do sistema com base no identificador fornecido. " +
        "Esta operação permite a exclusão de um contato existente, identificando-o pelo seu identificador único (GUID). " +
        "Antes da remoção, o sistema verifica se o contato existe para evitar inconsistências. " +
        "Se o contato for encontrado, a remoção será processada de forma assíncrona e o sistema retornará um status de sucesso (202 Accepted). " +
        "Caso o contato não seja encontrado ou os dados fornecidos sejam inválidos, retorna um erro de validação (400 BadRequest).";

    private static readonly Gauge MemoryUsageByEndpointGauge = Metrics.CreateGauge("api_memory_usage_by_endpoint_bytes", "Uso de memória da API por endpoint em bytes", new GaugeConfiguration
    {
        LabelNames = new[] { "endpoint" } // Usar 'endpoint' como label para diferenciar os diferentes endpoints
    });

    private readonly RemoverContatoCommandHandler _removerContatoCommandHandler;
    private readonly ILogger _logger;
    private readonly IValidator<RemoverContatoCommand> _validator;

    public ContatoController(ILoggerFactory loggerFactory, IValidator<RemoverContatoCommand> validator, RemoverContatoCommandHandler removerContatoCommandHandler)
    {
        _logger = loggerFactory.CreateLogger<ContatoController>();
        _validator = validator;
        _removerContatoCommandHandler = removerContatoCommandHandler;
    }
}