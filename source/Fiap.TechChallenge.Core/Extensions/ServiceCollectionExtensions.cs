using Fiap.TechChallenge.Core.Contracts.Commands.CommandValidators;
using Fiap.TechChallenge.Core.Data;
using Fiap.TechChallenge.Core.Data.QueryStores;
using Fiap.TechChallenge.Core.Data.Settings;
using Fiap.TechChallenge.Core.Handlers.CommandHandlers;
using Fiap.TechChallenge.Core.Messaging;
using Fiap.TechChallenge.Core.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.TechChallenge.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Settings

            var commandStoreSettings = new CommandStoreSettings();
            services.AddSingleton(commandStoreSettings);
            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseMySql(commandStoreSettings.SqlConnectionString, ServerVersion.AutoDetect(commandStoreSettings.SqlConnectionString))
            );

            #endregion

            #region Registrations

            services.AddSingleton<IMessageBrokerService, MessageBrokerService>();
            services.AddTransient<IContatoService, ContatoService>();
            services.AddTransient<IContatoQueryStore, ContatoQueryStore>();

            services.AddTransient<CriarContatoCommandHandler>();

            services.AddValidatorsFromAssemblyContaining<CriarContatoCommandValidator>();

            #endregion

            return services;
        }
    }
}
