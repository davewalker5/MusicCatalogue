using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Search
{
    [ExcludeFromCodeCoverage]
    public class AlbumSearchCriteria : MusicCatalogueEntityBase
    {
        public int? ArtistId { get; set; }
        public int? GenreId { get; set; }
        public bool? WishList { get; set; }
    }
}
