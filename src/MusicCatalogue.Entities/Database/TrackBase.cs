using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCatalogue.Entities.Database
{
    public abstract class TrackBase
    {
        public int? Duration { get; set; }

        /// <summary>
        /// Format the duration in MM:SS format
        /// </summary>
        /// <returns></returns>
        public string? FormattedDuration()
        {
            string? formatted = null;

            if (Duration != null)
            {
                int seconds = (Duration ?? 0) / 1000;
                int minutes = seconds / 60;
                seconds -= 60 * minutes;
                formatted = $"{minutes:00}:{seconds:00}";
            }

            return formatted;
        }
    }
}
