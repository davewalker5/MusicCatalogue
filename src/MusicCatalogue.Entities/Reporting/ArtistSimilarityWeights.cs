using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Reporting
{
    [ExcludeFromCodeCoverage]
    public class SimilarityWeights : MusicCatalogueEntityBase
    {
        // 1.0 = normal influence, 2.0 = double influence, 0.5 = half influence
        public double Energy { get; init; } = 1.0;
        public double Intimacy { get; init; } = 1.0;
        public double Warmth { get; init; } = 1.0;
        public double Mood {get; init; } = 1.0;
    }
}