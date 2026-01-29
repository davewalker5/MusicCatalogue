using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Reporting
{
    [ExcludeFromCodeCoverage]
    public sealed class ClosestArtist
    {
        public required Artist Artist { get; init; }
        
        // Raw distance (Euclidean or squared)
        public required double Distance { get; init; }

        // 0–100, higher = more similar
        public required double Similarity { get; init; }
    }
}