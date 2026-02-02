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
            // Configure the business logic factory
            var context = new MusicCatalogueDbContextFactory().CreateDbContext([]);
            var factory = new MusicCatalogueFactory(context);

            // Create a playlist
            var playlist = await factory.ArtistPlaylistBuilder.BuildPlaylist(PlaylistType.Curated, TimeOfDay.Evening, 5);
            var albums = await factory.ArtistPlaylistBuilder.PickPlaylistAlbums(playlist);

            foreach (var album in albums)
            {
                Console.WriteLine($"{album.Artist!.Name}, {album.Title}");
            }
        }
    }
}