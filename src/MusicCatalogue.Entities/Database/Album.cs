using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Extensions;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Album : PurchasableEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Artist.Id")]
        public int ArtistId { get; set; }

        [ForeignKey("Genre.Id")]
        public int? GenreId { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public int? Released { get; set; }

        public string? CoverUrl { get; set; } = "";
        public Genre? Genre { get; set; }

        public Artist? Artist { get; set; }
        public ICollection<Track>? Tracks { get; set; }

        [NotMapped]
        public long PlayingTime
            => Tracks?.Select(x => x.Duration ?? 0).Sum() ?? 0;

        [NotMapped]
        public string FormattedPlayingTime => DurationExtensions.DurationToFormattedPlayingTime(PlayingTime);
    }
}
