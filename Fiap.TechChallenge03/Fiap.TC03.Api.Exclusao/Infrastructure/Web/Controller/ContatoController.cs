using System.Diagnostics;
using System.Net;
using Fiap.TC03.Api.Exclusao.Domain.Command.Handler;
using Fiap.TC03.Api.Exclusao.Domain.Contract;
using Fiap.TechChallenge.Foundation.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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
    
    [Function("RemoverContatoPorId")]
    [OpenApiOperation("RemoverContatoPorId", "Fiap", Description = DescriptionRemocao)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Id do Contato.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(RemoverContatoCommandResult), Description = "The OK response")]
    public async Task<IActionResult> ObterContatoPorId([HttpTrigger(AuthorizationLevel.Function, "Delete", Route = "v1/contato/id/{id}")] HttpRequest req, string id)
    {
        var histogram = "remover_contato_latency_seconds";
        var endPoint = "RemoverContato";
        var counterName = "remover_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });
        
        try
        {
            var stopwatch = Stopwatch.StartNew();
            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);
            
            var command = new RemoverContatoCommand() { Id = id };
            await _validator.ValidateAndThrowAsync(command);
            var result = await _removerContatoCommandHandler.Handle(command);
            
            stopwatch.Stop();
            timer.ObserveDuration(); // Registra o tempo no Prometheus
            requestCounter.WithLabels("200").Inc(); // Incrementa contador para sucesso
            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            timer.ObserveDuration(); // Registra o tempo mesmo em caso de erro
            requestCounter.WithLabels("400").Inc(); // Incrementa contador para erro
            _logger.LogError($"Erro interno: {e.Message}", e);
            return new BadRequestObjectResult(new ErrorResponse(e.Message));
        }
    }
    
}