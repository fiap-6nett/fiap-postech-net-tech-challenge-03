using System.Diagnostics;
using System.Net;
using Fiap.TC03.Api.Consulta.Domain.Command.Handler;
using Fiap.TC03.Api.Consulta.Domain.Contato.Request;
using Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatoPorId;
using Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatosPorDdd;
using Fiap.TC03.Api.Consulta.Domain.Result;
using Fiap.TechChallenge.Foundation.Core.Enumerated;
using Fiap.TechChallenge.Foundation.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;

namespace Fiap.TC03.Api.Consulta.Infrastructure.Web.Controller.v1;

public class ConsultaController
{
    private const string DescriptionListagem =
        "Obtém a lista de todos os contatos cadastrados no sistema pelo DDD. " +
        "Esta operação retorna uma lista contendo todos os contatos registrados, permitindo paginação opcional " +
        "para melhor gerenciamento dos dados. " +
        "Os contatos são retornados com suas respectivas informações, incluindo nome, telefone, e-mail e DDD. " +
        "Se nenhum contato estiver cadastrado, retorna uma lista vazia.";
    
    private const string DescriptionObterContatoPorId =
        "Obtém os detalhes de um contato específico com base no identificador fornecido. " +
        "Esta operação consulta o contato associado ao ID fornecido e valida a entrada. " +
        "Se o contato for encontrado, suas informações serão retornadas. " +
        "Caso contrário, se o identificador for inválido ou o contato não existir, um erro apropriado será retornado.";

    
    
    private static readonly Gauge MemoryUsageByEndpointGauge = Metrics.CreateGauge("api_memory_usage_by_endpoint_bytes", "Uso de memória da API por endpoint em bytes", new GaugeConfiguration
    {
        LabelNames = new[] { "endpoint" } // Usar 'endpoint' como label para diferenciar os diferentes endpoints
    });

    private readonly ILogger _logger;
    private readonly ObterContatoPorIdQueryHandler _obterContatoPorIdQueryHandler;
    private readonly ObterContatosPorDddQueryHandler _obterContatosPorDddQueryHandler;
    private readonly IValidator<ObterContatoPorIdQueryRequest> _validatorObterContatoPorIdQuery;
    private readonly IValidator<ObterContatosPorDddQueryRequest> _validatorObterContatosPorDddQuery;

    public ConsultaController(ILoggerFactory loggerFactory, ObterContatoPorIdQueryHandler obterContatoPorIdQueryHandler, ObterContatosPorDddQueryHandler obterContatosPorDddQueryHandler, IValidator<ObterContatoPorIdQueryRequest> validatorObterContatoPorIdQuery,
        IValidator<ObterContatosPorDddQueryRequest> validatorObterContatosPorDddQuery)
    {
        _logger = loggerFactory.CreateLogger<ConsultaController>();
        _obterContatoPorIdQueryHandler = obterContatoPorIdQueryHandler;
        _obterContatosPorDddQueryHandler = obterContatosPorDddQueryHandler;
        _validatorObterContatoPorIdQuery = validatorObterContatoPorIdQuery;
        _validatorObterContatosPorDddQuery = validatorObterContatosPorDddQuery;
    }
    
    [Function("ObterContatosPorDdd")]
    [OpenApiOperation("ObterContatosPorDdd", "Fiap", Description = DescriptionListagem)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter("ddd", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "DDD do contato.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(ObterContatosPorDddResult), Description = "The OK response")]
    public async Task<IActionResult> ObterContatosPorDdd([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/contato/ddd/{ddd}")] HttpRequest req, int ddd)
    {
        var histogram = "consultar_contato_latency_seconds";
        var endPoint = "ConsultarContato";
        var counterName = "consultar_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });
        
        try
        {
            var stopwatch = Stopwatch.StartNew();
            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);
            
            var command = new ObterContatosPorDddQueryRequest{Ddd = ddd};
            await _validatorObterContatosPorDddQuery.ValidateAndThrowAsync(command);
            var result = await _obterContatosPorDddQueryHandler.Handle(command);
            
            stopwatch.Stop();
            timer.ObserveDuration(); // Registra o tempo no Prometheus
            requestCounter.WithLabels("202").Inc(); // Incrementa contador para sucesso
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
    
    [Function("ObterContatoPorId")]
    [OpenApiOperation("ObterContatoPorId", "Fiap", Description = DescriptionObterContatoPorId)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Id do Contato.")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(ObterContatoPorIdResult), Description = "The OK response")]
    public async Task<IActionResult> ObterContatoPorId([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/contato/id/{id}")] HttpRequest req, string id)
    {
        var histogram = "consultar_contato_latency_seconds";
        var endPoint = "ConsultarContato";
        var counterName = "consultar_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });
        
        try
        {
            var stopwatch = Stopwatch.StartNew();
            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);
            
            var command = new ObterContatoPorIdQueryRequest { Id = id };
            await _validatorObterContatoPorIdQuery.ValidateAndThrowAsync(command);
            var result = await _obterContatoPorIdQueryHandler.Handle(command);
            
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