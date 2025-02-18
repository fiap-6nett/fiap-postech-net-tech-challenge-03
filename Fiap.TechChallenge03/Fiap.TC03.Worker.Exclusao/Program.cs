using Fiap.TC03.Infrastructure.DbContexts;
using Fiap.TC03.Infrastructure.Interfaces;
using Fiap.TC03.Infrastructure.Repositories;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

// Configurar a string de conexão MySQL no appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar o DbContext para usar o MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddTransient<IContatoRepository, ContatoRepository>();

builder.Build().Run();