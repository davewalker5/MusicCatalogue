using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Reporting
{
    [Keyless]
    [ExcludeFromCodeCoverage]
    public class MonthlySpend : ReportEntityBase
    {
        [Export("Year", 1)]
        public int? Year { get; set; }

        [Export("Month", 2)]
        public int? Month { get; set; }

        [Export("Spend", 2)]
        public decimal Spend { get; set; }
    }
}
