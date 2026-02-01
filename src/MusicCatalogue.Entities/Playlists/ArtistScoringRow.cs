using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class ArtistScoringRow : MusicCatalogueEntityBase
    {
        public Artist? Artist { get; set; }
        public double MoodScore { get; set; }
        public double MoodScoreNorm { get; set; }
        public double StyleFit { get; set; }
        public double BaseScore { get; set; }
    }
}
