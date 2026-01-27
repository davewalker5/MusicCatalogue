using MusicCatalogue.Entities.DataExchange;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ITrackExporter
    {
        event EventHandler<TrackDataExchangeEventArgs>? TrackExport;

        Task Export(string file);
    }
}