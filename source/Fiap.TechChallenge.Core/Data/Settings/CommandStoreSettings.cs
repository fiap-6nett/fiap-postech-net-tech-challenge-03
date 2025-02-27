using Microsoft.Extensions.Configuration;

namespace Fiap.TechChallenge.Core.Data.Settings
{
    public class CommandStoreSettings
    {
        public string SqlConnectionString { get; }

        public CommandStoreSettings(IConfiguration configuration)
        {
            SqlConnectionString = configuration.GetConnectionString("FDataBase_SqlConnectionString");
        }
    }
}
