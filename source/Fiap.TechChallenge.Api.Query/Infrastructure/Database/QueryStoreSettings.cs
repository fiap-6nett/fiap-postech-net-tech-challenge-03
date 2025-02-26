namespace Fiap.TechChallenge.Api.Query.Infrastructure.Database;

public class CommandStoreSettings
{
    public CommandStoreSettings()
    {
        SqlConnectionString = Environment.GetEnvironmentVariable("FDataBase_SqlConnectionString");
    }

    public string SqlConnectionString { get; set; }
}