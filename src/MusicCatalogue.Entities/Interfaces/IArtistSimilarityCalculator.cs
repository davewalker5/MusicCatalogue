using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IArtistSimilarityCalculator
    {
        Task<List<ClosestArtist>> GetClosestArtistsAsync(
            SimilarityWeights weights,
            int targetArtistId,
            int n,
            bool excludeTarget = true);

        List<ClosestArtist> GetClosestArtists(
            IEnumerable<Artist> artists,
            SimilarityWeights weights,
            int targetArtistId,
            int n,
            bool excludeTarget = true);

        List<ClosestArtist> GetClosestArtists(
            IEnumerable<Artist> artists,
            SimilarityWeights weights,
            Artist target,
            int n,
            bool excludeTarget = true);
    }
}