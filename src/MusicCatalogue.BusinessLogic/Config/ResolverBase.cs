namespace MusicCatalogue.BusinessLogic.Config
{
    public abstract class ResolverBase
    {
        /// <summary>
        /// Resolve a value given the value from the configuration file
        /// </summary>
        /// <param name="configValue"></param>
        /// <returns></returns>
        public static string ResolveValue(string configValue)
        {
            string resolvedValue;

            // If the value from the configuration file is a valid file path, the actual value
            // is stored separately in the file indicated. This separation allows secrets not to
            // be published as part of the API or UI Docker container images but read from volume
            // mounts
            if (File.Exists(configValue))
            {
                resolvedValue = File.ReadAllText(configValue);
            }
            else
            {
                // Not a path to a file, so just return the configuration value
                resolvedValue = configValue;
            }

            return resolvedValue;
        }
    }
}
