using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Reporting
{
    [ExcludeFromCodeCoverage]
    public class SimilarityWeights : MusicCatalogueEntityBase
    {
        // 1.0 = normal influence, 2.0 = double influence, 0.5 = half influence
        public double EnergyWeight { get; set; }
        public double IntimacyWeight { get; set; }
        public double WarmthWeight { get; set; }
        public double MoodWeight {get; set; }
    }
}