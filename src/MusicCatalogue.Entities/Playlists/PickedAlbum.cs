using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class PickedAlbum : MusicCatalogueEntityBase
    {
        public required Album Album { get; set; }

        // Raw distance (Euclidean or squared)
        public double Distance { get; set; }

        // 0–100, higher = more similar
        public double Similarity { get; set; }

        // Internal similarity calculation results
        public double NumericDistance { get; set; }
        public double MoodDistance { get; set; }
        public int SharedMoods { get; set; }
    }
}