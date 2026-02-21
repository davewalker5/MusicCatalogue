using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public abstract class EquipmentBase : MusicCatalogueEntityBase
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("EquipmentType")]
        public int EquipmentTypeId { get; set; }

        [ForeignKey("Manufacturer")]
        public int ManufacturerId { get; set; }

        [Required]
        public string Description { get; set; } = "";

        public string? Model { get; set; }

        public string? SerialNumber { get; set; }
    }
}
