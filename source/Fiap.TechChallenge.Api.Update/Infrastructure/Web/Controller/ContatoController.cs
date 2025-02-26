using System.Diagnostics;
using System.Net;
using Fiap.TC03.Api.Atualizacao.Domain.Command.Handler;
using Fiap.TC03.Api.Atualizacao.Domain.Contract;
using Fiap.TechChallenge.Foundation.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
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
    
    [Function("AtualizarContatoCommand")]
    [OpenApiOperation("AtualizarContatoCommand", "Fiap", Description = DescriptionAtualizacao)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody("application/json", typeof(AtualizarContatoCommand), Description = "Parameters", Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.Accepted, "application/json", typeof(AtualizarContatoCommandResult), Description = "The OK response")]
    public async Task<IActionResult> AtualizarContatoCommand([HttpTrigger(AuthorizationLevel.Function, "put", Route = "v1/contato")] HttpRequest req)
    {
        var histogram = "atualizar_contato_latency_seconds";
        var endPoint = "AtualizarContato";
        var counterName = "atualizar_contato_requests_total";

        // Métrica personalizada para latência
        var timer = Metrics.CreateHistogram(histogram, $"Latência do endpoint {endPoint} em segundos").NewTimer();

        // Métrica personalizada para contagem de status
        var requestCounter = Metrics.CreateCounter(counterName, $"Total de requisições ao endpoint {endPoint}", new CounterConfiguration { LabelNames = new[] { "status" } });

        try
        {
            var stopwatch = Stopwatch.StartNew();
            // Atualiza a métrica de uso de memória
            MemoryUsageByEndpointGauge.WithLabels(endPoint).Set(Process.GetCurrentProcess().WorkingSet64);

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AtualizarContatoCommand>(requestBody);
            await _validator!.ValidateAndThrowAsync(data);
            var result = await _atualizarContatoCommandHandler.Handle(data);
            
            stopwatch.Stop();
            timer.ObserveDuration(); // Registra o tempo no Prometheus
            requestCounter.WithLabels("202").Inc(); // Incrementa contador para sucesso
            return new AcceptedResult("", result);
        }
        catch (ValidationException valEx)
        {
            timer.ObserveDuration(); // Registra o tempo mesmo em caso de erro
            requestCounter.WithLabels("400").Inc(); // Incrementa contador para erro
            
            _logger.LogError($"Erro de validação: {valEx.Message}", valEx);
            return new BadRequestObjectResult(valEx.Message);
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