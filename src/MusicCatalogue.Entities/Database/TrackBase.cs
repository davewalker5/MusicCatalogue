using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Extensions;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public abstract class TrackBase : MusicCatalogueEntityBase
    {
        protected const string DateTimeFormat = "dd/MM/yyyy";

        public int? Duration { get; set; }

        /// <summary>
        /// Duration formatted as MM:SS
        /// </summary>
        [NotMapped]
        public string? FormattedDuration => DurationExtensions.TrackDurationToString(Duration);

        public DateTime? Purchased { get; set; }

        /// <summary>
        /// Purchase date formatted per the DateTimeFormat
        /// </summary>
        public string FormattedPurchaseDate
        {
            get
            {
                return Purchased != null ? (Purchased ?? DateTime.Now).ToString(DateTimeFormat) : "";
            }
        }
    }
}
