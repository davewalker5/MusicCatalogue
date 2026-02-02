using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class ClosestArtistSearchCriteria : SimilarityWeights
    {
        public int ArtistId { get; set; }
        public int TopN { get; set; }
    }
}
