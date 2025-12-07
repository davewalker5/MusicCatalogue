using MusicCatalogue.Entities.DataExchange;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ICsvExporter<T> where T : class
    {
        event EventHandler<ExportEventArgs<T>> RecordExport;
        void Export(IEnumerable<T> entities, string fileName, char separator);
    }
}
