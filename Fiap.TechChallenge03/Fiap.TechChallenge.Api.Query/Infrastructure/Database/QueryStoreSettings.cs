namespace Fiap.TC03.Api.Consulta.Infrastructure.Database;

public class CommandStoreSettings
{
    public CommandStoreSettings()
    {
        SqlConnectionString = Environment.GetEnvironmentVariable("FDataBase_SqlConnectionString");
    }

    public string SqlConnectionString { get; set; }
}