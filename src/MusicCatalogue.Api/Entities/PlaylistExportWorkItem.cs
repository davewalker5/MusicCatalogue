using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Api.Entities
{
    public class PlaylistExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";
        public Playlist? Playlist { get; set; }
    }
}
