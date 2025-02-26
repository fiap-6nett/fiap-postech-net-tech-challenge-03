using Fiap.TechChallenge.Api.Delete.Domain.Command.Handler;
using Fiap.TechChallenge.Api.Delete.Domain.Contato;
using Fiap.TechChallenge.Api.Delete.Domain.Contract;
using Fiap.TechChallenge.Api.Delete.Infrastructure.MessageBroker;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
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
                Title = "API de Exclusao - Tech Challenge 03",
                Description = "Esta API permite o exclusao de contatos no sistema, seguindo a arquitetura de microsserviços e utilizando RabbitMQ para comunicação assíncrona.",
                Contact = new OpenApiContact
                {
                    Name = "Equipe Tech Challenge 03",
                    Email = "suporte@techchallenge03.com"
                }
            },
            OpenApiVersion = OpenApiVersionType.V3
        });
        
        // Configuração da conexão com RabbitMQ
        services.AddSingleton<IConnection>(sp =>
        {
            var settings = new MessageBrokerSettings();
            var factory = settings.CreateConnectionFactory();
            return factory.CreateConnection();
        });

        // Registra a Service que usa RabbitMQ
        services.AddSingleton<IMessageBrokerService, MessageBrokerService>();
        
        //  Service
        services.AddTransient<IContatoService, ContatoService>();
        services.AddTransient<IMessageBrokerService, MessageBrokerService>();
        
        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<RemoverContatoCommandValidator>();

        //  Handler
        services.AddTransient<RemoverContatoCommandHandler>();
    })
    .Build();

host.Run();