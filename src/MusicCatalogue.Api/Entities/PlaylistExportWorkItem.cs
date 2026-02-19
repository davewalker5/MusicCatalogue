using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Api.Entities
{
    [Obsolete("Playlist export will be replaced with a saved session export")]
    public class PlaylistExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";
        public Playlist? Playlist { get; set; }
    }
}
