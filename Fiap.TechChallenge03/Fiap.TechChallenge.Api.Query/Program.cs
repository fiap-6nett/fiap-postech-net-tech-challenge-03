using Fiap.TC03.Api.Consulta.Domain.Command.Handler;
using Fiap.TC03.Api.Consulta.Domain.Contato;
using Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatoPorId;
using Fiap.TC03.Api.Consulta.Domain.Contract.ObterContatosPorDdd;
using Fiap.TC03.Api.Consulta.Domain.DataBaseContext;
using Fiap.TC03.Api.Consulta.Infrastructure.Database;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

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
                Title = "API de Consulta - Tech Challenge 03",
                Description = "Esta API permite o consultar de contatos no sistema, seguindo a arquitetura de microsserviços e utilizando RabbitMQ para comunicação assíncrona.",
                Contact = new OpenApiContact
                {
                    Name = "Equipe Tech Challenge 03",
                    Email = "suporte@techchallenge03.com"
                }
            },
            OpenApiVersion = OpenApiVersionType.V3
        });


        //  Obtém a ConnectionString do ambiente
        var commandStoreSettings = new CommandStoreSettings();

        //  Configuração correta do DbContextFactory
        services.AddDbContextFactory<AppDbContext>(options => options.UseMySql(commandStoreSettings.SqlConnectionString, ServerVersion.AutoDetect(commandStoreSettings.SqlConnectionString)));

        //  Service
        services.AddTransient<IContatoService, ContatoService>();
        services.AddTransient<IContatoQueryStore, ContatoQueryStore>();

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<ObterContatoPorIdQueryValidator>();
        services.AddValidatorsFromAssemblyContaining<ObterContatosPorDddQueryValidator>();

        //  Handler
        services.AddTransient<ObterContatosPorDddQueryHandler>();
        services.AddTransient<ObterContatoPorIdQueryHandler>();
    })
    .Build();

host.Run();