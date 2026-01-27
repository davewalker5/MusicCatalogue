using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Search
{
    [ExcludeFromCodeCoverage]
    public class GenreSearchCriteria : MusicCatalogueEntityBase
    {
        public bool? WishList { get; set; }
    }
}
