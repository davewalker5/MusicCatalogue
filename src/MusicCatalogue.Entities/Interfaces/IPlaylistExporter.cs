using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IPlaylistExporter
    {
        event EventHandler<PlaylistDataExchangeEventArgs>? PlaylistItemExport;

        void Export(string file, Playlist playlist);
    }
}