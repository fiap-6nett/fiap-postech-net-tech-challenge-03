using RabbitMQ.Client;

namespace Fiap.TC03.Api.Exclusao.Infrastructure.MessageBroker;

public class MessageBrokerSettings
{
    public readonly string Hostname;
    public readonly string Password;
    public readonly string UserName;
    public readonly string VirtualHost;
    public readonly string ConnectionUri;

    public MessageBrokerSettings()
    {
        Hostname = Environment.GetEnvironmentVariable("FMessageBroker_Hostname");
        Password = Environment.GetEnvironmentVariable("FMessageBroker_Password");
        UserName = Environment.GetEnvironmentVariable("FMessageBroker_UserName");
        VirtualHost = Environment.GetEnvironmentVariable("FMessageBroker_VirtualHost");
        
        // Criando URI de conexão para simplificar
        ConnectionUri = $"amqps://{UserName}:{Password}@{Hostname}/{VirtualHost.TrimStart('/')}";
    }
    
    public ConnectionFactory CreateConnectionFactory()
    {
        return new ConnectionFactory
        {
            Uri = new Uri(ConnectionUri), // Usa a URI completa
            AutomaticRecoveryEnabled = true,
            TopologyRecoveryEnabled  = true,
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