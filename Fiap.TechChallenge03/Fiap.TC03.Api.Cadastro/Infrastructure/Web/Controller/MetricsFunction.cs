using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Prometheus;

namespace Fiap.TC03.Api.Cadastro.Infrastructure.Web.Controller
{
    public static class MetricsFunction
    {
        private static readonly CollectorRegistry Registry = Metrics.DefaultRegistry;

        [Function("Metrics")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "metrics")] HttpRequestData req)
        {
            using var stream = new MemoryStream();
            await Metrics.DefaultRegistry.CollectAndExportAsTextAsync(stream);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            var metricsContent = await reader.ReadToEndAsync();
            return new ContentResult { Content = metricsContent, ContentType = "text/plain" };
        }
    }
}

