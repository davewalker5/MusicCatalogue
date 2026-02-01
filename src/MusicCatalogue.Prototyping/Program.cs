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

            List<string> lines = ["Playlist,Time Of Day,Artist"];

            // Iterate over the times of day
            foreach (var tod in Enum.GetValues<TimeOfDay>())
            {
                // Playlist creation parameters
                var numberOfEntries = new[] { TimeOfDay.Evening, TimeOfDay.Late }.Contains(tod) ? 5 : 10;
                var numberOfPlaylists = 100;

                for (int i = 0; i < numberOfPlaylists; i++)
                {
                    var playlist = await factory.ArtistPlaylistBuilder.BuildCuratedArtistPlaylist(tod, numberOfEntries);
                    lines.AddRange(playlist.Select(x => $"{i},{tod},{x.ArtistName}"));
                }

            }

            // Write the CSV file
            File.WriteAllLines("playlists.csv", lines!);
        }
    }
}