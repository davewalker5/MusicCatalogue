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
        /// <returns></returns>
        public async Task PopulateArtistStatistics(IEnumerable<Artist> artists)
        {
            // Iterate over the supplied list
            foreach (var artist in artists)
            {
                // Get the albums for the current artist
                var albums = await _factory.Albums.ListAsync(x => x.ArtistId == artist.Id);

                // Count the albums and tracks
                artist.AlbumCount = albums.Count;
                artist.TrackCount = (albums.Count > 0) ? albums.Sum(x => x.Tracks.Count) : 0;
            }
        }
    }
}
