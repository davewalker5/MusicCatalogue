using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Search
{
    [ExcludeFromCodeCoverage]
    public class ArtistSearchCriteria : MusicCatalogueEntityBase
    {
        public string? NamePrefix { get; set; }
        public bool? WishList { get; set; }
        public int? GenreId { get; set; }
        public bool? IncludeArtistsWithNoAlbums { get; set; }
    }
}
