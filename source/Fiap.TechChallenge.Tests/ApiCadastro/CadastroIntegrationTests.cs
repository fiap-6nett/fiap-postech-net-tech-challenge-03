using Fiap.TechChallenge.Api.Create.Infrastructure.MessageBroker;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;

namespace Fiap.TechChallenge.Test.ApiCadastro;

public class CadastroIntegrationTests
{
    private readonly MessageBrokerService _messageBrokerService;
    private readonly Mock<IConnection> _mockConnection;
    private readonly Mock<ILogger<MessageBrokerService>> _mockLogger;

    public CadastroIntegrationTests()
    {
        _mockConnection = new Mock<IConnection>();
        _mockLogger = new Mock<ILogger<MessageBrokerService>>();

        Environment.SetEnvironmentVariable("FMessageBroker_Hostname", "toucan-01.lmq.cloudamqp.com");
        Environment.SetEnvironmentVariable("FMessageBroker_Password", "EgeNXUTA7cp9ownXvg8XOQESox2N9Rbc");
        Environment.SetEnvironmentVariable("FMessageBroker_UserName", "wesrhrfp");
        Environment.SetEnvironmentVariable("FMessageBroker_VirtualHost", "wesrhrfp");

        _messageBrokerService = new MessageBrokerService(_mockConnection.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ProducerAsync_ValidMessage_ReturnsTrue()
    {
        var queueName = "test-queue";
        var message = "test-message";

        var mockChannel = new Mock<IModel>();
        _mockConnection.Setup(c => c.CreateModel()).Returns(mockChannel.Object);

        var result = await _messageBrokerService.ProducerAsync(queueName, message);

        Assert.True(result);
    }
}