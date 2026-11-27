namespace HotelBooking.Api.Extensions;

internal static class ConfigurationExtensions
{
    extension(IConfigurationBuilder configurationBuilder)
    {
        internal IConfigurationBuilder AddModuleConfiguration(string[] modules, string environmentName)
        {
            foreach (var module in modules)
            {
                configurationBuilder.AddJsonFile($"modules.{module}.json", optional: false, reloadOnChange: true);
                configurationBuilder.AddJsonFile($"modules.{module}.{environmentName}.json", optional: true, reloadOnChange: true);
            }

            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder;
        }
    }
}
