using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.DataExchange;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IPlaylistExporter
    {
        event EventHandler<PlaylistDataExchangeEventArgs>? PlaylistItemExport;

        void Export(string file, IList<Album> playlist);
    }
}