using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Equipment : PurchasableEntity
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

        public EquipmentType? EquipmentType { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }
}
