using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.Api.Entities
{
    [ExcludeFromCodeCoverage]
    public class ClosestArtistSearchCriteria : SimilarityWeights
    {
        public int ArtistId { get; set; }
        public int TopN { get; set; }
    }
}
