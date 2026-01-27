using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.Config
{
    public class MusicCatalogueConfigReader : ConfigReader<MusicApplicationSettings>, IConfigReader<MusicApplicationSettings>
    {
        /// <summary>
        /// Load and return the application settings from the named JSON-format application settings file
        /// </summary>
        /// <param name="jsonFileName"></param>
        /// <returns></returns>
        public override MusicApplicationSettings? Read(string jsonFileName)
        {
            // Read the settings
            var settings = base.Read(jsonFileName);
            if (settings != null)
            {
                // Resolve all the API keys for services where the key is held in a separate file
                ApiKeyResolver.ResolveAllApiKeys(settings);

                // Repeat for the secrets
                SecretResolver.ResolveAllSecrets(settings!);
            }

            return settings;
        }
    }
}
