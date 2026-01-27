using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public abstract class NamedEntity : MusicCatalogueEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";
    }
}
