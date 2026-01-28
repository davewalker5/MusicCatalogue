using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Artist : NamedEntity
    {

        [ForeignKey("Vibe.Id")]
        public int? VibeId { get; set; }
        public string? SearchableName { get; set; } = null;
        public int Energy { get; set; } = 0;
        public int Intimacy { get; set; } = 0;
        public int Warmth { get; set; } = 0;

        public VocalPresence Vocals { get; set; } = VocalPresence.Unknown;
        public EnsembleType Ensemble { get; set; } = EnsembleType.Unknown;

        public ICollection<Album>? Albums { get; set; }
    }
}
