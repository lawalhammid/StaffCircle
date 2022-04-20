using Microsoft.Extensions.Configuration;

namespace api.Configuartions
{
    public static class AppSettingsConfig
    {
        public static string CorsPolicyAppSettings()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");

            IConfiguration configuration = configurationBuilder.Build();

            string prefix = configuration["CorsPolicy"];
            return prefix;
        }

        public static string EnvironmentAppSettings()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("AppSettings.json");
           
            IConfiguration configuration = configurationBuilder.Build();

            string prefix = configuration["Environment"];
            return prefix;
        }
    }
}
