using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class ArtistMood : MusicCatalogueEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public int MoodId { get; set; }

        public Mood? Mood { get; set; }
    }
}
