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
    }
}
