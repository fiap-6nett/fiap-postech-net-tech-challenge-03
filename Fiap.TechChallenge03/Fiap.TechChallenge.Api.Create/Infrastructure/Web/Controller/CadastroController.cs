using System.Diagnostics;
using System.Net;
using Fiap.TC03.Api.Cadastro.Domain.Command.Handler;
using Fiap.TC03.Api.Cadastro.Domain.Contract.CriarContato;
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

namespace Fiap.TC03.Api.Cadastro.Infrastructure.Web.Controller;

public class CadastroController
{
    private const string DescriptionCadastro =
        "Cria um novo contato no sistema. " +
        "Esta operação permite a criação de um novo contato com as informações fornecidas, " +
        "como nome, telefone, e-mail e DDD (Discagem Direta à Distância). " +
        "O contato é validado antes de ser processado, garantindo que os dados estejam consistentes. " +
        "O objeto de entrada deve conter os seguintes campos: " +
        "- Nome: Nome completo do contato. " +
        "- Telefone: Número de telefone do contato. " +
        "- Email: Endereço de e-mail do contato. " +
        "- DDD: Código da região a que o contato pertence. " +
        "Retorna um status de sucesso (202 OK) se o contato for criado com êxito. " +
        "Caso contrário, retorna um erro de validação (400 BadRequest) caso algum dos dados fornecidos seja inválido.";


    private static readonly Gauge MemoryUsageByEndpointGauge = Metrics.CreateGauge("api_memory_usage_by_endpoint_bytes", "Uso de memória da API por endpoint em bytes", new GaugeConfiguration
    {
        LabelNames = new[] { "endpoint" } // Usar 'endpoint' como label para diferenciar os diferentes endpoints
    });

    private readonly CriarContatoCommandHandler _criarContatoCommandHandler;
    private readonly ILogger _logger;
    private readonly IValidator<CriarContatoCommand> _validatorCriarContatoCommand;

    public CadastroController(ILoggerFactory loggerFactory, IValidator<CriarContatoCommand> validatorCriarContatoCommand, CriarContatoCommandHandler criarContatoCommandHandler)
    {
        _logger = loggerFactory.CreateLogger<CadastroController>();
        _validatorCriarContatoCommand = validatorCriarContatoCommand;
        _criarContatoCommandHandler = criarContatoCommandHandler;
    }

    /// <summary>
    ///     Cria um novo contato no sistema.
    /// </summary>
    /// <remarks>
    ///     Esta operação permite a criação de um novo contato com as informações fornecidas,
    ///     como nome, telefone, e-mail e DDD (Discagem Direta à Distância).
    ///     O contato é validado antes de ser processado, garantindo que os dados estejam consistentes.
    /// </remarks>
    /// <param name="command">
    ///     Objeto que contém as informações do contato a ser criado. Deve incluir os seguintes campos:
    ///     - Nome: Nome completo do contato.
    ///     - Telefone: Número de telefone do contato.
    ///     - Email: Endereço de e-mail do contato.
    ///     - DDD: Código da região a que o contato pertence.
    /// </param>
    /// <returns>
    ///     Retorna um status de sucesso (202 OK) se o contato for criado com êxito.
    ///     Caso contrário, retorna um erro de validação (400 BadRequest) caso algum dos dados fornecidos seja inválido.
    /// </returns>
    /// <response code="202">Contato criado com sucesso.</response>
    /// <response code="400">Erro nos dados fornecidos, como falha na validação dos campos.</response>
    [Function("CriarContatoCommand")]
    [OpenApiOperation("CriarContatoCommand", "Fiap", Description = DescriptionCadastro)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody("application/json", typeof(CriarContatoCommand), Description = "Parameters", Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.Accepted, "application/json", typeof(CriarContatoCommandResult), Description = "The OK response")]
    public async Task<IActionResult> CriarContatoCommand([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/contato")] HttpRequest req)
    {
        var histogram = "criar_contato_latency_seconds";
        var endPoint = "CriarContato";
        var counterName = "criar_contato_requests_total";

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
            var data = JsonConvert.DeserializeObject<CriarContatoCommand>(requestBody);
            await _validatorCriarContatoCommand!.ValidateAndThrowAsync(data);
            var result = await _criarContatoCommandHandler.Handle(data);
            
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