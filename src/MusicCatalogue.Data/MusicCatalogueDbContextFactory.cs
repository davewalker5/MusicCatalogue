using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Data
{
    public class MusicCatalogueDbContextFactory : IDesignTimeDbContextFactory<MusicCatalogueDbContext>
    {
        /// <summary>
        /// Create a context for the real database 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public MusicCatalogueDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

            // Construct a configuration object that contains the key/value pairs from the settings file
            // at the root of the main applicatoin
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                                    .AddJsonFile("appsettings.json")
                                                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                                                    .Build();

            // Use the configuration object to read the connection string
            var optionsBuilder = new DbContextOptionsBuilder<MusicCatalogueDbContext>();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("MusicCatalogueDB"));

            // Construct and return a database context
            return new MusicCatalogueDbContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Create an in-memory context for unit testing
        /// </summary>
        /// <returns></returns>
        public static MusicCatalogueDbContext CreateInMemoryDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicCatalogueDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return new MusicCatalogueDbContext(optionsBuilder.Options);
        }
    }
}
