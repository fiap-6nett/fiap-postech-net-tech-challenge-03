namespace Fiap.TechChallenge.Core.Messaging
{
    public interface IMessageBrokerService
    {
        Task<bool> ProducerAsync(string queueName, string message);
    }
}
