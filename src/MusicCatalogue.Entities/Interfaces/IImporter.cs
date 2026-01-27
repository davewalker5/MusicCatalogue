using MusicCatalogue.Entities.DataExchange;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IImporter
    {
        event EventHandler<TrackDataExchangeEventArgs>? TrackImport;
        Task Import(string file);
    }
}