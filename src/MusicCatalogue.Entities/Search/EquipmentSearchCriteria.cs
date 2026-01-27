using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Search
{
    [ExcludeFromCodeCoverage]
    public class EquipmentSearchCriteria : MusicCatalogueEntityBase
    {
        public int? EquipmentTypeId { get; set; }
        public int? ManufacturerId { get; set; }
        public bool? WishList { get; set; }
    }
}
