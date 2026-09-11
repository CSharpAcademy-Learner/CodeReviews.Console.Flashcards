using Microsoft.Extensions.Configuration;

namespace Flashcards.CSharpAcademyLearner.Configuration
{
    internal static class ConfigurationManager
    {
        internal static readonly IConfigurationRoot _config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", false, true).Build();

        internal static string GetMasterConnectionString()
        {
            string? masterString = _config.GetConnectionString("MasterConnection");

            if (string.IsNullOrEmpty(masterString))
            {
                throw new InvalidOperationException("Connection string 'MasterConnection' is missing from appsettings.json");
            }

            return masterString;
        }

        internal static string GetAppConnectionString()
        {
            string? appString = _config.GetConnectionString("AppConnection");

            if (string.IsNullOrEmpty(appString))
            {
                throw new InvalidOperationException("Connection string 'AppConnection' is missing from appsettings.json");
            }

            return appString;
        }
    }
}
