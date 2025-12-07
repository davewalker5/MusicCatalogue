using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.DataExchange
{

    [ExcludeFromCodeCoverage]
    public class EquipmentDataExchangeEventArgs : EventArgs
    {
        public long RecordCount { get; set; }
        public FlattenedEquipment? Equipment { get; set; }
    }
}
