using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.DataExchange
{
    [ExcludeFromCodeCoverage]
    public class ExportEventArgs<T> : EventArgs where T : class
    {
        public long RecordCount { get; set; }
        public T? RecordSource { get; set; }
    }
}
