using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Reporting
{
    [Keyless]
    [ExcludeFromCodeCoverage]
    public class RetailerStatistics : ReportEntityBase
    {
        [Export("Name", 1)]
        public string Name { get; set; } = "";

        [Export("Artists", 2)]
        public int? Artists { get; set; }

        [Export("Albums", 3)]
        public int? Albums { get; set; }

        [Export("Tracks", 4)]
        public int? Tracks { get; set; }

        [Export("Spend", 5)]
        public decimal? Spend { get; set; }
    }
}
