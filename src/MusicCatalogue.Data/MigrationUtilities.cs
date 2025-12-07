using System.Reflection;

namespace MusicCatalogue.Data
{
    public static class MigrationUtilities
    {
        /// <summary>
        /// Get the namespace for the migration SQL scripts
        /// </summary>
        /// <returns></returns>
        public static string GetSqlScriptNamespace()
        {
            // The assumption is that there will be a SQL folder at the same level as this class
            // where the SQL script resources are held
            var dataAssemblyNamespace = MethodBase.GetCurrentMethod()?.DeclaringType?.Namespace ?? "";
            var sqlScriptNamespace = $"{dataAssemblyNamespace}.Sql";
            return sqlScriptNamespace;
        }

        /// <summary>
        /// Read and return the contents of an embedded resource containing a SQL migration script
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ReadMigrationSqlScript(string name)
        {
            string content = "";

            // Get the resource name
            var sqlScriptNamespace = GetSqlScriptNamespace();
            var sqlResourceName = $"{sqlScriptNamespace}.{name}";

            // Get the name of the resource and a resource stream for reading it
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream(sqlResourceName);

            // Open a stream reader to read the file content
            using (var reader = new StreamReader(resourceStream!))
            {
                // Read the file content
                content = reader.ReadToEnd();
            }

            return content;
        }
    }
}
