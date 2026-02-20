using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.DataExchange
{

    [ExcludeFromCodeCoverage]
    public class SessionDataExchangeEventArgs : EventArgs
    {
        public long RecordCount { get; set; }
        public FlattenedSessionAlbum? Item { get; set; }
    }
}
