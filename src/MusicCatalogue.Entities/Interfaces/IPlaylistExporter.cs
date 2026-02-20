using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.DataExchange;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ISessionExporter
    {
        event EventHandler<SessionDataExchangeEventArgs>? SessionAlbumExport;

        void Export(string file, Session session);
    }
}