using Fiap.TechChallenge.Core.Contracts.Requests;
using Fiap.TechChallenge.Core.Data.CommandStores;
using Fiap.TechChallenge.Core.Services;
using Fiap.TechChallenge.Worker.Delete.DTOs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Fiap.TechChallenge.Worker.Delete.QueueFunctions
{
    public class RemoverContatoQueueFunction
    {
        private readonly ILogger<RemoverContatoQueueFunction> _logger;
        private readonly IContatoCommandStore _contatoCommandStore;

        private const string RabbitMqUri = "amqps://wesrhrfp:EgeNXUTA7cp9ownXvg8XOQESox2N9Rbc@toucan.lmq.cloudamqp.com/wesrhrfp";
        private const string QueueName = "fiap-remover";

        public RemoverContatoQueueFunction(ILogger<RemoverContatoQueueFunction> logger, IContatoCommandStore contatoCommandStore)
        {
            _logger = logger;
            _contatoCommandStore = contatoCommandStore;
        }

        [Function("RemoverContatoQueueFunction")]
        public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo timer)
        {
            try
            {
                var factory = new ConnectionFactory { Uri = new Uri(RabbitMqUri) };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                var result = channel.BasicGet(QueueName, autoAck: true);

                if (result == null)
                {
                    _logger.LogInformation("Não há mensagens na fila");
                    return;
                }

                var message = Encoding.UTF8.GetString(result.Body.ToArray());
                var contato = JsonSerializer.Deserialize<RemoverContatoDTO>(message);
                if (contato == null)
                {
                    _logger.LogWarning("Falha ao desserializar a mensagem");
                    return;
                }

                bool sucesso = await _contatoCommandStore.RemoverContatoAsync(contato.Id);

                if (sucesso)
                {
                    _logger.LogInformation($"Contato {contato.Id} removido");
                }
                else
                {
                    _logger.LogWarning($"Contato {contato.Id} não pôde ser removido");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar a fila: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }
}
