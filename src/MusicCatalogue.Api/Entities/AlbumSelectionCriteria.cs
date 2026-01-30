using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.Api.Entities
{
    [ExcludeFromCodeCoverage]
    public class AlbumSelectionCrtieria : SimilarityWeights
    {
        public int? GenreId { get; set; }
    }
}