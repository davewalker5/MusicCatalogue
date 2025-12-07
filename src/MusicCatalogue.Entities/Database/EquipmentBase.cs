using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public abstract class EquipmentBase
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("EquipmentType.Id")]
        public int EquipmentTypeId { get; set; }

        [ForeignKey("Manufacturer.Id")]
        public int ManufacturerId { get; set; }

        [Required]
        public string Description { get; set; } = "";

        public string? Model { get; set; }

        public string? SerialNumber { get; set; }
    }
}
