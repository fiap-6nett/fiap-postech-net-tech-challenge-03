using Fiap.TC03.Infrastructure.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Fiap.TC03.Worker.Exclusao.Functions
{
    public class DeleteProcessQueue
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly ILogger _logger;

        public DeleteProcessQueue(IContatoRepository contatoRepository, ILoggerFactory loggerFactory)
        {
            _contatoRepository = contatoRepository;
            _logger = loggerFactory.CreateLogger<DeleteProcessQueue>();
        }

        [Function("DeleteProcessQueue")]
        public async Task Run([QueueTrigger("fiap-remover", Connection = "AzureWebJobsStorage")] string queueItem)
        {
            _logger.LogInformation($"Processando item da fila: {queueItem}");

            var contatoId = JsonSerializer.Deserialize<Guid>(queueItem);
            _contatoRepository.Delete(contatoId);

            await Task.CompletedTask;
        }
    }
}
