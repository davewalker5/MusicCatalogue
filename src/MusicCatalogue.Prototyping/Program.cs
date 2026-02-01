using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.BusinessLogic.Logging;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Playlists;

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
            var logger = new ConsoleLogger();
            logger.Initialise("", Severity.Debug);

            // Configure the business logic factory
            var context = new MusicCatalogueDbContextFactory().CreateDbContext([]);
            var factory = new MusicCatalogueFactory(context);

            // Build a curated playlist
            var playlist = await factory.ArtistPlaylistBuilder.BuildCuratedArtistPlaylist(TimeOfDay.Evening, 10);
            Console.WriteLine($"\nCurated:\n");
            foreach (var row in playlist)
            {
                Console.WriteLine($"\t{row.ArtistName}");
            }

            // Build a normal playlist
            playlist = await factory.ArtistPlaylistBuilder.BuildNormalArtistPlaylist(TimeOfDay.Evening, 10);
            Console.WriteLine($"\n\nNormal:\n");
            foreach (var row in playlist)
            {
                Console.WriteLine($"\t{row.ArtistName}");
            }
        }
    }
}