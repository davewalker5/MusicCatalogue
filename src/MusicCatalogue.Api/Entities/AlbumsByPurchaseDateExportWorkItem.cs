using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Api.Entities
{
    [ExcludeFromCodeCoverage]
    public class AlbumsByPurchaseDateExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
