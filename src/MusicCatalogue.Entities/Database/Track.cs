using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Track : TrackBase
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Album")]
        [Required]
        public int AlbumId { get; set; }

        public int? Number { get; set; }

        [Required]
        public string Title { get; set; } = "";
    }
}
