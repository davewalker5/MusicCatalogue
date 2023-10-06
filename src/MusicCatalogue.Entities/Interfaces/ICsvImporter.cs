using MusicCatalogue.Entities.DataExchange;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ICsvImporter
    {
        event EventHandler<TrackDataExchangeEventArgs>? TrackImport;
        Task Import(string file);
    }
}