using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.BusinessLogic.Logging;
using MusicCatalogue.Data;
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
            // Configure the business logic factory
            var context = new MusicCatalogueDbContextFactory().CreateDbContext([]);
            var logger = new ConsoleLogger();
            var factory = new MusicCatalogueFactory(context, logger);

            List<string> lines = ["Playlist,Time Of Day,Type,Artist"];

            // Iterate over the times of day
            foreach (var tod in Enum.GetValues<TimeOfDay>())
            {
                // Playlist creation parameters
                var numberOfEntries = new[] { TimeOfDay.Evening, TimeOfDay.Late }.Contains(tod) ? 5 : 10;
                var numberOfPlaylists = 500;

                for (int i = 0; i < numberOfPlaylists; i++)
                {
                    // Alternate between tightly curated and "normal" playlists
                    var type = i %2 == 0 ? PlaylistType.Curated : PlaylistType.Normal;
                    var number = type == PlaylistType.Curated ? 5 : 10;
                    var playlist = await factory.PlaylistBuilder.BuildPlaylistAsync(type, tod, null, number, [], []);
                    lines.AddRange(playlist.Albums.Select(x => $"{i},{tod},{type},{x.Artist!.Name}"));
                }
            }

            // Write the CSV file
            File.WriteAllLines("playlists.csv", lines!);
        }
    }
}