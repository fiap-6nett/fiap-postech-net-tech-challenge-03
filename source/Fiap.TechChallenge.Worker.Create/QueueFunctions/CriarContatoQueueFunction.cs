using Fiap.TechChallenge.Core.Data.CommandStores;
using Fiap.TechChallenge.Core.Entities;
using Fiap.TechChallenge.Worker.Create.DTOs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Fiap.TechChallenge.Worker.Create.QueueFunctions
{
    public class CriarContatoQueueFunction
    {
        private readonly ILogger<CriarContatoQueueFunction> _logger;
        private readonly IContatoCommandStore _contatoCommandStore;

        private const string RabbitMqUri = "amqps://wesrhrfp:EgeNXUTA7cp9ownXvg8XOQESox2N9Rbc@toucan.lmq.cloudamqp.com/wesrhrfp";
        private const string QueueName = "fiap-cadastro";
        private const string DlqQueueName = "dlq_fiap-cadastro";

        public CriarContatoQueueFunction(ILogger<CriarContatoQueueFunction> logger, IContatoCommandStore contatoCommandStore)
        {
            _logger = logger;
            _contatoCommandStore = contatoCommandStore;
        }

        [Function("CriarContatoQueueFunction")]
        public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo timer)
        {
            var factory = new ConnectionFactory { Uri = new Uri(RabbitMqUri) };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var result = channel.BasicGet(QueueName, autoAck: false);
            if (result == null)
            {
                _logger.LogInformation("Não há mensagens na fila.");
                return;
            }

            try
            {
                var message = Encoding.UTF8.GetString(result.Body.ToArray());
                var contatoDto = JsonSerializer.Deserialize<CriarContatoDTO>(message);
                if (contatoDto == null)
                {
                    _logger.LogWarning("Falha ao desserializar a mensagem");
                    channel.BasicNack(result.DeliveryTag, false, false);
                    return;
                }

                var contato = new ContatoEntity(contatoDto.Id, contatoDto.Nome, contatoDto.Telefone, contatoDto.Email, contatoDto.DDD);

                bool sucesso = await _contatoCommandStore.CriarContatoAsync(contato);

                if (sucesso)
                {
                    _logger.LogInformation($"Contato {contato.Id} criado");
                    channel.BasicAck(result.DeliveryTag, false);
                }
                else
                {
                    _logger.LogWarning($"Contato {contato.Id} não pôde ser criado");
                    channel.BasicNack(result.DeliveryTag, false, false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar a fila: {ex.Message}");
                channel.BasicNack(result.DeliveryTag, false, false);
            }

            await Task.CompletedTask;
        }
    }
}
