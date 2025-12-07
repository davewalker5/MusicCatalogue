using MusicCatalogue.Entities.DataExchange;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IExporter
    {
        event EventHandler<TrackDataExchangeEventArgs>? TrackExport;

        Task Export(string file);
    }
}