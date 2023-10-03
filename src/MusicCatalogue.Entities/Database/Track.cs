using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Track
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Album.Id")]
        [Required]
        public int AlbumId { get; set; }

        public int? Number { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public int? Duration { get; set; }

#pragma warning disable CS8618
        public Album Album { get; set; }
#pragma warning restore CS8618

        /// <summary>
        /// Format the duration in MM:SS format
        /// </summary>
        /// <returns></returns>
        public string? FormattedDuration()
        {
            string? formatted = null;

            if (Duration != null)
            {
                int seconds = (Duration ?? 0) / 1000;
                int minutes = seconds / 60;
                seconds -= 60 * minutes;
                formatted = $"{minutes:00}:{seconds:00}";
            }

            return formatted;
        }
    }
}
