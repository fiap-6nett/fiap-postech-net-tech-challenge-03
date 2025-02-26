namespace Fiap.TechChallenge.Api.Update.Infrastructure.MessageBroker;

public interface IMessageBrokerService
{
    Task<bool> ProducerAsync(string queueName, string message);
}