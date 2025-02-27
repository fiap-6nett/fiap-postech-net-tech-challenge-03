namespace Fiap.TechChallenge.Core.Data.Settings
{
    public class CommandStoreSettings
    {
        public CommandStoreSettings()
        {
            SqlConnectionString = Environment.GetEnvironmentVariable("FDataBase_SqlConnectionString");
        }

        public string SqlConnectionString { get; set; }
    }
}
