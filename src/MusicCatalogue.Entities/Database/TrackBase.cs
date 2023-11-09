﻿using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public abstract class TrackBase
    {
        public int? Duration { get; set; }
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
    }
}
