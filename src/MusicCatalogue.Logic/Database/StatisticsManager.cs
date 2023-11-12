using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.Logic.Database
{
    public class StatisticsManager : IStatisticsManager
    {
        private readonly IMusicCatalogueFactory _factory;

        internal StatisticsManager(IMusicCatalogueFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Populate the album and track counts on each artist in the specified collection
        /// </summary>
        /// <param name="artists"></param>
        /// <param name="wishlist"></param>
        /// <returns></returns>
        public async Task PopulateArtistStatistics(IEnumerable<Artist> artists, bool wishlist)
        {
            // Iterate over the supplied list
            foreach (var artist in artists)
            {
                List<Album> albums;

                // Make sure the tracks and albums are loaded - the wish list flag is either null, false or true
                if (wishlist)
                {
                    albums = await _factory.Albums.ListAsync(x => (x.ArtistId == artist.Id) && (x.IsWishListItem == true));
                }
                else
                {
                    albums = await _factory.Albums.ListAsync(x => (x.ArtistId == artist.Id) && (x.IsWishListItem != true));
                }

                // Count the albums and tracks
                artist.AlbumCount = albums!.Count;
                artist.TrackCount = (albums.Count > 0) ? albums.Sum(x => x.Tracks.Count) : 0;
                artist.TotalAlbumSpend = (albums.Count > 0) ? albums.Sum(x => x.Price ?? 0M) : 0;
            }
        }
    }
}
