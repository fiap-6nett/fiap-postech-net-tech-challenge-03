namespace Fiap.TechChallenge.Api.Create.Infrastructure.MessageBroker;

public interface IMessageBrokerService
{
    Task<bool> ProducerAsync(string queueName, string message);
}