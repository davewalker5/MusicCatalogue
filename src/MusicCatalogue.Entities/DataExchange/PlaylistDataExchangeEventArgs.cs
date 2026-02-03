using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.DataExchange
{

    [ExcludeFromCodeCoverage]
    public class PlaylistDataExchangeEventArgs : EventArgs
    {
        public long RecordCount { get; set; }
        public FlattenedPlaylistItem? Item { get; set; }
    }
}
