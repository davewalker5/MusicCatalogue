using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.Data;

namespace MusicCatalogue.Prototyping
{
    public static class Program
    {
        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            // Configure the business logic factory
            var context = new MusicCatalogueDbContextFactory().CreateDbContext([]);
            var factory = new MusicCatalogueFactory(context);
        }
    }
}