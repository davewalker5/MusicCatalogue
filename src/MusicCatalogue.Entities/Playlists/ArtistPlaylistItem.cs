using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class ArtistPlaylistItem : MusicCatalogueEntityBase
    {
        public int ArtistId { get; set; }
        public string? ArtistName { get; set; }
        public double StepScore { get; set; }
        public double BaseScore { get; set; }
        public double StyleFit { get; set; }
        public double MoodScore { get; set; }
        public double MoodScoreNorm { get; set; }
        public double Energy { get; set; }
        public double Intimacy { get; set; }
        public double Warmth { get; set; }
        public VocalPresence VocalPresence { get; set; }
        public EnsembleType EnsembleType { get; set; }
    }
}