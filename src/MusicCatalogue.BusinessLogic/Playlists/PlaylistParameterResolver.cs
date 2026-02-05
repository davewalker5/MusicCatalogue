using MusicCatalogue.Entities.Extensions;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.BusinessLogic.Playlists
{
    public static class PlaylistParameterResolver
    {
        private const double StyleWeight = 0.55;
        private const double MoodWeight = 0.45;

        /// <summary>
        /// Resolve the playlist builder parameters for a playlist type and time of day
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="currentArtistId"></param>
        /// <param name="numberOfEntries"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PlaylistParameters Resolve(PlaylistType type, TimeOfDay timeOfDay, int? currentArtistId, int numberOfEntries)
        {
            var basePreset = PlaylistPresetTable.GetBasePreset(type, timeOfDay);

            int topK = type switch
            {
                PlaylistType.Curated => NumberRangeExtensions.ClampInteger((int)Math.Round(basePreset.TopK + basePreset.Variety * 2.0), 3, 8),
                PlaylistType.Normal => NumberRangeExtensions.ClampInteger((int)Math.Round(basePreset.TopK * (0.75 + 0.75 * basePreset.Variety)), 3, 15),
                _ => throw new ArgumentOutOfRangeException()
            };

            double temperature = type switch
            {
                PlaylistType.Curated => NumberRangeExtensions.Clamp(basePreset.Temperature * (1.0 + 0.5 * basePreset.Variety), 0.30, 0.85),
                PlaylistType.Normal => NumberRangeExtensions.Clamp(basePreset.Temperature * (0.70 + 0.70 * basePreset.Variety), 0.35, 1.15),
                _ => throw new ArgumentOutOfRangeException()
            };

            double transitionPenalty = type switch
            {
                PlaylistType.Curated => NumberRangeExtensions.Clamp(basePreset.TransitionPenalty * (1.0 - 0.2 * basePreset.Variety), 0.18, 0.35),
                PlaylistType.Normal => basePreset.TransitionPenalty,
                _ => throw new ArgumentOutOfRangeException()
            };

            numberOfEntries = NumberRangeExtensions.ClampInteger(numberOfEntries, basePreset.MinimumEntries, numberOfEntries);

            return new PlaylistParameters(
                TimeOfDay: timeOfDay,
                CurrentArtistId: currentArtistId,
                NumberOfEntries: numberOfEntries,
                TopK: topK,
                Temperature: temperature,
                TransitionPenalty: transitionPenalty,
                StyleWeight: StyleWeight,
                MoodWeight: MoodWeight,
                AvoidRecent: basePreset.AvoidRecent,
                RandomJitter: basePreset.RandomJitter,
                Seed: null
            );
        }
    }
}