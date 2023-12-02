using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Search
{
    [ExcludeFromCodeCoverage]
    public class EquipmentSearchCriteria
    {
        public int? EquipmentTypeId { get; set; }
        public int? ManufacturerId { get; set; }
        public bool? WishList { get; set; }
    }
}
