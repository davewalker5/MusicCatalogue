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
            var factory = new MusicCatalogueFactory(context, new ConsoleLogger());

            // Build and display a playlist
            var playlist = await factory.ArtistPlaylistBuilder.BuildPlaylist(PlaylistType.Normal, TimeOfDay.Afternoon, 5);
            var albums = await factory.ArtistPlaylistBuilder.PickPlaylistAlbums(playlist);
            foreach (var album in albums)
            {
                Console.WriteLine($"{album.Title} - {album.Artist!.Name}");
            }
        }
    }
}