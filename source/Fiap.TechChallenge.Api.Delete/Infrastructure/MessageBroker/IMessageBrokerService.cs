namespace Fiap.TechChallenge.Api.Delete.Infrastructure.MessageBroker;

public interface IMessageBrokerService
{
    Task<bool> ProducerAsync(string queueName, string message);
}