using MusicCatalogue.Entities.Config;

namespace MusicCatalogue.Logic.Config
{
    public class ApiKeyResolver : ResolverBase
    {
        /// <summary>
        /// Resolve all the API key definitions in the supplied application settings
        /// </summary>
        /// <param name="settings"></param>
        public static void ResolveAllApiKeys(MusicApplicationSettings settings)
        {
            // Iterate over the service API key definitions
            foreach (var service in settings.ApiServiceKeys)
            {
                // Resolve the key for the current service
                service.Key = ResolveValue(service.Key);
            }
        }
    }
}
