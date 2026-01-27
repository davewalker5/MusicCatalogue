using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public abstract class TrackBase
    {
        protected const string DateTimeFormat = "dd/MM/yyyy";

        public int? Duration { get; set; }

        /// <summary>
        /// Duration formatted as MM:SS
        /// </summary>
        public string? FormattedDuration
        {
            get
            {
                if (Duration != null)
                {
                    int seconds = (Duration ?? 0) / 1000;
                    int minutes = seconds / 60;
                    seconds -= 60 * minutes;
                    return $"{minutes:00}:{seconds:00}";
                }
                else
                {
                    return null;
                }
            }
        }

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
