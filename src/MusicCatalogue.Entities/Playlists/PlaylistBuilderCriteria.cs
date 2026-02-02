using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class PlaylistBuilderCriteria : MusicCatalogueEntityBase
    {
        public PlaylistType Type { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public int NumberOfEntries { get; set; }
    }
}