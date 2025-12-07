using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Search
{
    [ExcludeFromCodeCoverage]
    public class AlbumSearchCriteria
    {
        public int? ArtistId { get; set; }
        public int? GenreId { get; set; }
        public bool? WishList { get; set; }
    }
}
