namespace Fiap.TechChallenge.Api.Create.Infrastructure.Database;

public class CommandStoreSettings
{
    public CommandStoreSettings()
    {
        SqlConnectionString = Environment.GetEnvironmentVariable("FDataBase_SqlConnectionString");
    }

    public string SqlConnectionString { get; set; }
}