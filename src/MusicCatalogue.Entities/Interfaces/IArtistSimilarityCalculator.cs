using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IArtistSimilarityCalculator
    {
        Task<List<ClosestArtist>> GetClosestArtistsAsync(
            int targetArtistId,
            int n,
            bool excludeTarget = true);

        List<ClosestArtist> GetClosestArtists(
            IEnumerable<Artist> artists,
            int targetArtistId,
            int n,
            bool excludeTarget = true);
    }
}