using Fiap.TechChallenge.Core.Contracts.Commands.CommandValidators;
using Fiap.TechChallenge.Core.Contracts.Queries.QueryValidators;
using Fiap.TechChallenge.Core.Data;
using Fiap.TechChallenge.Core.Data.CommandStores;
using Fiap.TechChallenge.Core.Data.QueryStores;
using Fiap.TechChallenge.Core.Data.Settings;
using Fiap.TechChallenge.Core.Handlers.CommandHandlers;
using Fiap.TechChallenge.Core.Handlers.QueryHandlers;
using Fiap.TechChallenge.Core.Messaging;
using Fiap.TechChallenge.Core.Messaging.Settings;
using Fiap.TechChallenge.Core.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Fiap.TechChallenge.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Settings

            var commandStoreSettings = new CommandStoreSettings(configuration);
            services.AddSingleton(commandStoreSettings);

            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseMySql(commandStoreSettings.SqlConnectionString, ServerVersion.AutoDetect(commandStoreSettings.SqlConnectionString))
            );

            services.AddSingleton<MessageBrokerSettings>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new MessageBrokerSettings(configuration);
            });

            // Configuração da conexão com RabbitMQ
            services.AddSingleton<IConnection>(sp =>
            {
                var settings = sp.GetRequiredService<MessageBrokerSettings>();
                var factory = settings.CreateConnectionFactory();
                return factory.CreateConnection();
            });

            #endregion

            #region Registrations

            services.AddSingleton<IMessageBrokerService, MessageBrokerService>();
            services.AddTransient<IContatoService, ContatoService>();
            services.AddTransient<IContatoQueryStore, ContatoQueryStore>();
            services.AddTransient<IContatoCommandStore, ContatoCommandStore>();

            services.AddTransient<CriarContatoCommandHandler>();
            services.AddTransient<RemoverContatoCommandHandler>();
            services.AddTransient<ObterContatosPorDddQueryHandler>();
            services.AddTransient<ObterContatoPorIdQueryHandler>();
            services.AddTransient<AtualizarContatoCommandHandler>();

            services.AddValidatorsFromAssemblyContaining<CriarContatoCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<RemoverContatoCommandValidator>();
            services.AddTransient<ObterContatoPorIdQueryValidator>();
            services.AddTransient<ObterContatosPorDddQueryValidator>();
            services.AddTransient<AtualizarContatoCommandValidator>();

            #endregion

            return services;
        }
    }
}
