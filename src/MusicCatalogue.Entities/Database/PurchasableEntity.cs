using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public abstract class PurchasableEntity : MusicCatalogueEntityBase
    {
        public bool? IsWishListItem { get; set; }

        public DateTime? Purchased { get; set; }

        public decimal? Price { get; set; }

        public int? RetailerId { get; set; }

        public Retailer? Retailer { get; set; }
    }
}
