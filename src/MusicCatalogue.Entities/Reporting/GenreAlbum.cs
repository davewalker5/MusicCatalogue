using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Reporting
{
    [Keyless]
    [ExcludeFromCodeCoverage]
    public class GenreAlbum : ReportEntityBase
    {
        [Export("Artist", 1)]
        public string Artist { get; set; } = "";

        [Export("Title", 2)]
        public string Title { get; set; } = "";

        [Export("Genre", 3)]
        public string Genre { get; set; } = "";

        [Export("Released", 4)]
        public int Released { get; set; }

        [Export("Purchased", 5)]
        public DateTime Purchased { get; set; }

        [Export("Price", 6)]
        public Decimal Price { get; set; }

        [Export("Retailer", 7)]
        public string Retailer { get; set; } = "";
    }
}
