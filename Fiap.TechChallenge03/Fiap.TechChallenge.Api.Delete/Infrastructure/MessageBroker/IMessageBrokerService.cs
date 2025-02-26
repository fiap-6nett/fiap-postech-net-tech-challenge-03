namespace Fiap.TC03.Api.Exclusao.Infrastructure.MessageBroker;

public interface IMessageBrokerService
{
    Task<bool> ProducerAsync(string queueName, string message);
}