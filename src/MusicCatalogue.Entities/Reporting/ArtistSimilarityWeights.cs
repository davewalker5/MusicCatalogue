namespace MusicCatalogue.Entities.Reporting
{
    public class SimilarityWeights
    {
        // 1.0 = normal influence, 2.0 = double influence, 0.5 = half influence
        public double Energy { get; init; } = 1.0;
        public double Intimacy { get; init; } = 1.0;
        public double Warmth { get; init; } = 1.0;
    }
}