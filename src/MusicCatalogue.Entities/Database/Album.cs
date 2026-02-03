using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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
        public int PlayingTime
            => Tracks?.Select(x => x.Duration ?? 0).Sum() ?? 0;

        /// <summary>
        /// Total playing time formatted as HH:MM:SS
        /// </summary>
        [NotMapped]
        public string FormattedPlayingTime
        {
            get
            {
                int seconds = PlayingTime / 1000;
                int hours = seconds / 3600;
                seconds -= 3600 * hours;
                int minutes = seconds / 60;
                seconds -= 60 * minutes;
                return $"{hours:00}:{minutes:00}:{seconds:00}";
            }
        }

    }
}
