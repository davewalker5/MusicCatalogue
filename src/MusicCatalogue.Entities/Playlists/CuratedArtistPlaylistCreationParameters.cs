using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Extensions;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class CuratedArtistPlaylistCreationParameters : ArtistPlaylistCreationParameters
    {
        // Variety can be between 0 and 1 but should be low to keep the curated feel
        private const double PlaylistVariety = 0.4;
        private const int MinimumEntries = 3;

        /// <summary>
        /// Parameters for building a short playlist with a curated feel.
        /// </summary>
        /// <param name="timeOfDay"></param>
        /// <param name="variety"></param>
        /// <param name=""></param>
        public CuratedArtistPlaylistCreationParameters(TimeOfDay timeOfDay, int numberOfEntries)
        {
            var presets = timeOfDay switch
            {
                TimeOfDay.Morning   => (topK: 4, temp: 0.55, transitionPenalty: 0.22),
                TimeOfDay.Afternoon => (topK: 4, temp: 0.55, transitionPenalty: 0.22),
                TimeOfDay.Evening   => (topK: 3, temp: 0.45, transitionPenalty: 0.25),
                TimeOfDay.Late      => (topK: 2, temp: 0.35, transitionPenalty: 0.28),
                _ => (topK: 3, temp: 0.45, transitionPenalty: 0.25)
            };

            // The numbers, below, determine the "feel" of the playlist and are arrived at by trial-and-error,
            // with the intention of building a small playlist with:
            //
            // Strong coherence
            // Limited exploration
            // Smooth transitions between artists
            // Variation that increases gently but not explosively
            //
            // This is in contrast to the intent with a regular playlist where the intent is sustained listening
            // across a larger pool
            TimeOfDay = timeOfDay;
            NumberOfEntries = NumberRangeExtensions.ClampInteger(numberOfEntries, MinimumEntries, numberOfEntries);
            StyleWeight = 0.55;
            MoodWeight = 0.45;
            AvoidRecent = 10;
            TopK = NumberRangeExtensions.ClampInteger((int)Math.Round(presets.topK + (PlaylistVariety * 2.0)), 2, 6);
            Temperature = NumberRangeExtensions.Clamp(presets.temp * (1.0 + 0.5 * PlaylistVariety), 0.30, 0.85);
            TransitionPenalty = NumberRangeExtensions.Clamp(presets.transitionPenalty * (1.0 - 0.2 * PlaylistVariety), 0.18, 0.35);
            RandomJitter = 0.005;
            Seed = null;
        }

        /// <summary>
        /// Convenience wrapper to create a new instance of the parameters
        /// </summary>
        /// <param name="timeOfDay"></param>
        /// <param name="numberOfEntries"></param>
        /// <returns></returns>
        public static CuratedArtistPlaylistCreationParameters Create(TimeOfDay timeOfDay, int numberOfEntries)
            => new(timeOfDay, numberOfEntries);
    }
}