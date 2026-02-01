using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Extensions;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class NormalArtistPlaylistCreationParameters : ArtistPlaylistCreationParameters
    {
        // Variety can be between 0 and 1 but should be low to keep the curated feel
        private const double PlaylistVariety = 0.5;
        private const int MinimumEntries = 3;

        /// <summary>
        /// Parameters for building a short playlist with a curated feel.
        /// </summary>
        /// <param name="timeOfDay"></param>
        /// <param name="variety"></param>
        /// <param name=""></param>
        public NormalArtistPlaylistCreationParameters(TimeOfDay timeOfDay, int numberOfEntries)
        {
            var presets = timeOfDay switch
            {
                TimeOfDay.Morning   => (topK: 10, temp: 0.8, transitionPenalty: 0.18),
                TimeOfDay.Afternoon => (topK: 10, temp: 0.8, transitionPenalty: 0.18),
                TimeOfDay.Evening   => (topK:  8, temp: 0.7, transitionPenalty: 0.18),
                TimeOfDay.Late      => (topK:  6, temp: 0.6, transitionPenalty: 0.18),
                _ => (topK: 8, temp: 0.7, transitionPenalty: 0.18)
            };

            // The numbers, below, determine the "feel" of the playlist and are arrived at by trial-and-error,
            // so they deserve some explanation. The intention is to build a playlist where the intent is sustained
            // listening across a larger pool.
            // 
            // This is in contrast to a short playlist where the intent is a coherent playlist with limited
            // exploration and a curated feel.
            //
            // The higher values, below, compared to the short playlist parameters allow for greater stylistic
            // range and occasional contrast (transition penalty is low).
            TimeOfDay = timeOfDay;
            NumberOfEntries = NumberRangeExtensions.ClampInteger(numberOfEntries, MinimumEntries, numberOfEntries);
            StyleWeight = 0.55;
            MoodWeight = 0.45;
            AvoidRecent = 8;
            TopK = NumberRangeExtensions.ClampInteger((int)Math.Round(presets.topK * (0.75 + 0.75 * PlaylistVariety)), 3, 15);
            Temperature = NumberRangeExtensions.Clamp(presets.temp * (0.70 + 0.70 * PlaylistVariety), 0.35, 1.15);
            TransitionPenalty = presets.transitionPenalty;
            RandomJitter = 01;
            Seed = null;
        }

        /// <summary>
        /// Convenience wrapper to create a new instance of the parameters
        /// </summary>
        /// <param name="timeOfDay"></param>
        /// <param name="numberOfEntries"></param>
        /// <returns></returns>
        public static NormalArtistPlaylistCreationParameters Create(TimeOfDay timeOfDay, int numberOfEntries)
            => new(timeOfDay, numberOfEntries);
    }
}