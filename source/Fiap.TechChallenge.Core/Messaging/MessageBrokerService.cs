﻿using Fiap.TechChallenge.Core.Messaging.Settings;
using Fiap.TechChallenge.Foundation.Core.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace Fiap.TechChallenge.Core.Messaging
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly IConnection _connection;
        private readonly ILogger<MessageBrokerService> _logger;
        private readonly MessageBrokerSettings _messageBrokerConfiguration;

        public MessageBrokerService(IConnection connection, ILogger<MessageBrokerService> logger, MessageBrokerSettings messageBrokerConfiguration)
        {
            _connection = connection;
            _logger = logger;
            _messageBrokerConfiguration = messageBrokerConfiguration;
        }

        /// <summary>
        ///     Metodo para enviar item para fila
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ProducerAsync(string queueName, string message)
        {
            return await ExecuteWithRetryPolicyAsync(10, () =>
            {
                try
                {
                    using var channel = GetConnection().CreateModel();
                    var nameDlq = string.Concat("dlq_", queueName);
                    
                    // Tenta declarar a DLQ apenas se não existir
                    try
                    {
                        channel.QueueDeclarePassive(nameDlq); // Apenas verifica se existe
                    }
                    catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
                    {
                        // Se não existe, cria a fila DLQ corretamente
                        channel.QueueDeclare(nameDlq, true, false, false, null);
                    }
                    
                    // Criando a fila principal e vinculando com a DLQ
                    var args = new Dictionary<string, object>
                    {
                        { "x-dead-letter-exchange", "" },  // Sem exchange (padrão)
                        { "x-dead-letter-routing-key", nameDlq },  // Nome da fila DLQ
                        { "x-message-ttl", 1728000000 }  // 20 dias em milissegundos
                    };
                    
                    // Tenta declarar a fila principal apenas se não existir
                    try
                    {
                        channel.QueueDeclarePassive(queueName);
                    }
                    catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
                    {
                        // Se a fila não existe, criamos ela corretamente
                        channel.QueueDeclare(queueName, true, false, false, args);
                    }

                    var body = Encoding.UTF8.GetBytes(message);
                    var props = channel.CreateBasicProperties();
                    props.Persistent = true;

                    channel.BasicPublish("", queueName, props, body);
                    return Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    var mensagem = $"Erro ao enviar mensagem para a fila {queueName}";
                    _logger.LogError(mensagem, ex.Serialize());
                    throw new Exception(mensagem, ex);
                }
            });
        }

        /// <summary>
        ///     Metodo para executar com politica de retry
        /// </summary>
        /// <param name="quantityRetry"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private async Task<T> ExecuteWithRetryPolicyAsync<T>(int quantityRetry, Func<Task<T>> action)
        {
            var retryPolicy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetryAsync(quantityRetry, retryAttempt => TimeSpan.FromSeconds(2 * retryAttempt),
                    (exception, timeSpan, retryCount, context) => { _logger.LogError($"Tentativa {retryCount} falhou. Retentativa após {timeSpan.TotalSeconds} segundos devido a: {exception.Message}"); });

            return await retryPolicy.ExecuteAsync(action);
        }

        /// <summary>
        ///     Metodo para obter conexão com o RabbitMQ
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IConnection GetConnection()
        {
            try
            {
                return _connection is not { IsOpen: true } ? CreateConnection() : _connection;
            }
            catch (Exception e)
            {
                var mensagem = "Erro ao criar conexão com o RabbitMQ. Erro: " + e.Message;
                _logger.LogError(mensagem, e.Serialize());
                throw new Exception(mensagem, e);
            }
        }

        /// <summary>
        ///     Método para criar uma conexão com o RabbitMQ, com política de retry.
        /// </summary>
        /// <returns>Instância de IConnection</returns>
        /// <exception cref="Exception">Lança exceção se não for possível estabelecer a conexão após todas as tentativas.</exception>
        private IConnection CreateConnection()
        {
            // Define a política de retry com Polly
            var retryPolicy = Policy
                .Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(10, // Tenta 10 vezes
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Exponencial backoff
                    (exception, timeSpan, retryCount, context) => { _logger.LogWarning($"Tentativa {retryCount} de conexão falhou: {exception.GetType().Name} - {exception.Message}. " + $"Tentando novamente em {timeSpan.TotalSeconds} segundos."); });

            try
            {
                var factory = _messageBrokerConfiguration.CreateConnectionFactory();
                return retryPolicy.Execute(() => factory.CreateConnection());
            }
            catch (Exception e)
            {
                // Log o erro com detalhes e relança a exceção customizada
                var mensagem = "Erro ao criar conexão com o RabbitMQ. Erro: " + e.Message;
                _logger.LogError(mensagem, e.Serialize());
                throw new Exception(mensagem, e);
            }
        }
    }
}
