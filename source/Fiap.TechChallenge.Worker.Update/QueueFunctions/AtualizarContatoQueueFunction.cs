using Fiap.TechChallenge.Core.Data.CommandStores;
using Fiap.TechChallenge.Core.Entities;
using Fiap.TechChallenge.Worker.Update.DTOs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Fiap.TechChallenge.Worker.Update.QueueFunctions
{
    public class AtualizarContatoQueueFunction
    {
        private readonly ILogger<AtualizarContatoQueueFunction> _logger;
        private readonly IContatoCommandStore _contatoCommandStore;

        private const string RabbitMqUri = "amqps://wesrhrfp:EgeNXUTA7cp9ownXvg8XOQESox2N9Rbc@toucan.lmq.cloudamqp.com/wesrhrfp";
        private const string QueueName = "fiap-atualizar";

        public AtualizarContatoQueueFunction(ILogger<AtualizarContatoQueueFunction> logger, IContatoCommandStore contatoCommandStore)
        {
            _logger = logger;
            _contatoCommandStore = contatoCommandStore;
        }

        [Function("AtualizarContatoQueueFunction")]
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
                    _logger.LogInformation("Não há mensagens na fila.");
                    return;
                }

                var message = Encoding.UTF8.GetString(result.Body.ToArray());
                var contatoDto = JsonSerializer.Deserialize<AtualizarContatoDTO>(message);
                if (contatoDto == null)
                {
                    _logger.LogWarning("Falha ao desserializar a mensagem.");
                    return;
                }

                var contato = new ContatoEntity(
                    contatoDto.Id,
                    contatoDto.Nome,
                    contatoDto.Telefone,
                    contatoDto.Email,
                    contatoDto.DDD);

                bool sucesso = await _contatoCommandStore.AtualizarContatoAsync(contato);

                if (sucesso)
                {
                    _logger.LogInformation($"Contato {contato.Id} atualizado com sucesso.");
                }
                else
                {
                    _logger.LogWarning($"Contato {contato.Id} não pôde ser atualizado.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar a fila: {ex.Message}", ex);
            }
        }
    }
}
