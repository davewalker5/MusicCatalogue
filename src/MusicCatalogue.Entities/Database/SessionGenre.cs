using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class SessionGenre : MusicCatalogueEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int SessionId { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public bool Include { get; set; }

        public Genre? Genre { get; set; }
    }
}