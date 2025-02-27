using Fiap.TechChallenge.Core.Extensions;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services.AddCoreServices(builder.Configuration);

builder.ConfigureFunctionsWebApplication();

builder.Build().Run();
