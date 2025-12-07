using MusicCatalogue.Entities.Config;

namespace MusicCatalogue.Logic.Config
{
    public class SecretResolver : ResolverBase
    {
        /// <summary>
        /// Resolve all the API key definitions in the supplied application settings
        /// </summary>
        /// <param name="settings"></param>
        public static void ResolveAllSecrets(MusicApplicationSettings settings)
        {
            // Iterate over the secret definitions
            foreach (var secret in settings.Secrets)
            {
                // Resolve the value for the current secret
                secret.Value = ResolveValue(secret.Value);
            }
        }
    }
}
