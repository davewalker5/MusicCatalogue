using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Retailer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set;}
        public string? WebSite { get; set; }
    }
}
