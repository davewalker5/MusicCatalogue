using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Search
{
    [ExcludeFromCodeCoverage]
    public class SessionSearchCriteria : MusicCatalogueEntityBase
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public PlaylistType? Type { get; set; }
        public TimeOfDay? TimeOfDay { get; set; }
    }
}