using Microsoft.Extensions.Configuration;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.Config
{
    public abstract class ConfigReader<T> : IConfigReader<T> where T : class
    {
        /// <summary>
        /// Load and return the application settings from the named JSON-format application settings file
        /// </summary>
        /// <returns></returns>
        public virtual T? Read(string jsonFileName)
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            var directory = Path.GetDirectoryName(jsonFileName);
            var environmentFileName = $"{Path.GetFileNameWithoutExtension(jsonFileName)}.{environment}{Path.GetExtension(jsonFileName)}";
            var environmentFilePath = Path.Combine(directory ?? string.Empty, environmentFileName);

            // Set up the configuration reader
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(jsonFileName)
                .AddJsonFile(environmentFilePath, optional: true)
                .Build();

            // Read the application settings section
            IConfigurationSection section = configuration.GetSection("ApplicationSettings");
            var settings = section.Get<T>();

            return settings;
        }
    }
}
