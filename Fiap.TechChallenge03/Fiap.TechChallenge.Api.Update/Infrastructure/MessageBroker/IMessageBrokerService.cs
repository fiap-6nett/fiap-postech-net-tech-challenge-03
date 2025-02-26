namespace Fiap.TC03.Api.Atualizacao.Infrastructure.MessageBroker;

public interface IMessageBrokerService
{
    Task<bool> ProducerAsync(string queueName, string message);
}