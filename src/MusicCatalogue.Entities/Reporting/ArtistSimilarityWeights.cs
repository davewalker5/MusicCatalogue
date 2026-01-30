using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Reporting
{
    [ExcludeFromCodeCoverage]
    public class SimilarityWeights : MusicCatalogueEntityBase
    {
        // 1.0 = normal influence, 2.0 = double influence, 0.5 = half influence
        public double Energy { get; init; }
        public double Intimacy { get; init; }
        public double Warmth { get; init; }
        public double Mood {get; init; }
    }
}