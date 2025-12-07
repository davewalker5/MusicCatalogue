using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Search
{
    [ExcludeFromCodeCoverage]
    public class ArtistSearchCriteria
    {
        public string? NamePrefix { get; set; }
        public bool? WishList { get; set; }
        public int? GenreId { get; set; }
        public bool? IncludeArtistsWithNoAlbums { get; set; }
    }
}
