using Fiap.TechChallenge.Core.Extensions;
using Fiap.TechChallenge.Core.Messaging.Settings;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;

var host = new HostBuilder()
.ConfigureFunctionsWebApplication()
.ConfigureServices((context, services) =>
{
    #region Settings

    services.AddApplicationInsightsTelemetryWorkerService();
    services.ConfigureFunctionsApplicationInsights();

    services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
    });

    // Configure os serviços necessários para OpenApi
    services.AddSingleton<IOpenApiConfigurationOptions>(_ => new DefaultOpenApiConfigurationOptions
    {
        // The important parts:
        IncludeRequestingHostName = false,
        Servers = [new OpenApiServer { Url = "/api" }],

        // Optional settings:
        Info =
        {
            Version = "1.0.0", // Version of your API
            Title = "API de Cadastro - Tech Challenge 03",
            Description = "Esta API permite o cadastro de contatos no sistema, seguindo a arquitetura de microsserviços e utilizando RabbitMQ para comunicação assíncrona.",
            Contact = new OpenApiContact
            {
                Name = "Equipe Tech Challenge 03",
                Email = "suporte@techchallenge03.com"
            }
        },
        OpenApiVersion = OpenApiVersionType.V3
    });

    #endregion

    #region Registrations

    services.AddCoreServices(context.Configuration);

    #endregion

}).Build();

host.Run();