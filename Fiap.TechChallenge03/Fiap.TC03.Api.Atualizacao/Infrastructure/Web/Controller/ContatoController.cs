using Fiap.TC03.Api.Atualizacao.Domain.Command.Handler;
using Fiap.TC03.Api.Atualizacao.Domain.Contract;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace Fiap.TC03.Api.Atualizacao.Infrastructure.Web.Controller;

public class ContatoController
{
    private const string DescriptionAtualizacao =
        "Atualiza as informações de um contato existente no sistema. " +
        "Esta operação permite modificar os dados de um contato previamente cadastrado, " +
        "como nome, telefone, e-mail e DDD (Discagem Direta à Distância). " +
        "O contato a ser atualizado é identificado pelo seu identificador único (GUID). " +
        "Os novos dados são validados antes de serem processados para garantir consistência. " +
        "O objeto de entrada deve conter os seguintes campos: " +
        "- Nome: Nome completo atualizado do contato. " +
        "- Telefone: Número de telefone atualizado do contato. " +
        "- Email: Endereço de e-mail atualizado. " +
        "- DDD: Código da região atualizado do contato. " +
        "A atualização do contato é enviada para processamento assíncrono, retornando um status de sucesso (202 Accepted). " +
        "Caso os dados fornecidos sejam inválidos, retorna um erro de validação (400 BadRequest).";

    private static readonly Gauge MemoryUsageByEndpointGauge = Metrics.CreateGauge("api_memory_usage_by_endpoint_bytes", "Uso de memória da API por endpoint em bytes", new GaugeConfiguration
    {
        LabelNames = new[] { "endpoint" } // Usar 'endpoint' como label para diferenciar os diferentes endpoints
    });

    private readonly AtualizarContatoCommandHandler _atualizarContatoCommandHandler;
    private readonly ILogger _logger;
    private readonly IValidator<AtualizarContatoCommand> _validator;

    public ContatoController(ILoggerFactory loggerFactory, IValidator<AtualizarContatoCommand> validator, AtualizarContatoCommandHandler autalizarContatoCommandHandler)
    {
        _logger = loggerFactory.CreateLogger<ContatoController>();
        _validator = validator;
        _atualizarContatoCommandHandler = autalizarContatoCommandHandler;
    }
}