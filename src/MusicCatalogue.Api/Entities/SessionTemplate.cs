using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Api.Entities
{
    public class SessionTemplate : MusicCatalogueEntityBase
    {
        public PlaylistType Type { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public ICollection<int> AlbumIds { get; set; } = [];
    }
}