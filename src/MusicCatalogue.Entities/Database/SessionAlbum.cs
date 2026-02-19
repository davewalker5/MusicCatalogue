using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class SessionAlbum : MusicCatalogueEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int SessionId { get; set; }

        [ForeignKey("Album")]
        public int AlbumId { get; set; }
        public int Position { get; set; }

        public Album? Album { get; set; }
    }
}