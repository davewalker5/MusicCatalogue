using MusicCatalogue.Entities.DataExchange;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IEquipmentExporter
    {
        event EventHandler<EquipmentDataExchangeEventArgs>? EquipmentExport;

        Task Export(string file);
    }
}