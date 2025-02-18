using Fiap.TC03.Infrastructure.Interfaces;
using Fiap.TC03.Worker.Exclusao.DTOs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Fiap.TC03.Worker.Exclusao.Functions
{
    public class DeleteProcessQueue
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly ILogger _logger;

        private const string RabbitMqUri = "amqps://wesrhrfp:EgeNXUTA7cp9ownXvg8XOQESox2N9Rbc@toucan.lmq.cloudamqp.com/wesrhrfp";
        private const string QueueName = "fiap-remover";

        public DeleteProcessQueue(IContatoRepository contatoRepository, ILoggerFactory loggerFactory)
        {
            _contatoRepository = contatoRepository;
            _logger = loggerFactory.CreateLogger<DeleteProcessQueue>();
        }

        [Function("DeleteProcessQueue")]
        public async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo timer)
        {
            _logger.LogInformation("Verificando mensagens na fila RabbitMQ.");

            var factory = new ConnectionFactory
            {
                Uri = new Uri(RabbitMqUri)
            };

            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    var result = channel.BasicGet(QueueName, autoAck: true);
                    if (result != null)
                    {
                        var message = Encoding.UTF8.GetString(result.Body.ToArray());
                        _logger.LogInformation($"Mensagem recebida: {message}");

                        var contatoMsg = JsonSerializer.Deserialize<ExcluirContatoDTO>(message);
                        if (contatoMsg != null)
                        {
                            _contatoRepository.Delete(contatoMsg.Id);
                            _logger.LogInformation($"Contato com Id {contatoMsg.Id} deletado.");
                        }
                        else
                        {
                            _logger.LogWarning("Falha ao desserializar a mensagem.");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Nenhuma mensagem encontrada na fila.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagens do RabbitMQ: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }
}
