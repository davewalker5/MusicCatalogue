using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.DataExchange
{
    [ExcludeFromCodeCoverage]
    public class TrackDataExchangeEventArgs : EventArgs
    {
        public long RecordCount { get; set; }
        public FlattenedTrack? Track { get; set; }
    }
}
