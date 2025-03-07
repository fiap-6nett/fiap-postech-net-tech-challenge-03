using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Fiap.TechChallenge.Core.Messaging.Settings
{
    public class MessageBrokerSettings
    {
        public readonly string Hostname;
        public readonly string Password;
        public readonly string UserName;
        public readonly string VirtualHost;
        public string ConnectionUri => $"amqps://{UserName}:{Password}@{Hostname}/{VirtualHost.TrimStart('/')}";

        public MessageBrokerSettings(IConfiguration configuration)
        {
            Hostname = Environment.GetEnvironmentVariable("FMessageBroker_Hostname");
            Password = Environment.GetEnvironmentVariable("FMessageBroker_Password");
            UserName = Environment.GetEnvironmentVariable("FMessageBroker_UserName");
            VirtualHost = Environment.GetEnvironmentVariable("FMessageBroker_VirtualHost");
        }

        public ConnectionFactory CreateConnectionFactory()
        {
            return new ConnectionFactory
            {
                Uri = new Uri(ConnectionUri), // Usa a URI completa
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                DispatchConsumersAsync = true,
                Ssl = new SslOption
                {
                    ServerName = Hostname,
                    Enabled = true
                }
            };
        }
    }
}
