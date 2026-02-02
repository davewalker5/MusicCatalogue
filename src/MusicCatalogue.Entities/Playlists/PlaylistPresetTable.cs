using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public static class PlaylistPresetTable
    {
        public static PlaylistPresetBase GetBasePreset(PlaylistType mode, TimeOfDay timeOfDay)
            => (mode, timeOfDay) switch
            {
                (PlaylistType.Curated, TimeOfDay.Morning)   => new(4, 0.55, 0.22, 0.4, 3, 10, 0.02),
                (PlaylistType.Curated, TimeOfDay.Afternoon) => new(4, 0.55, 0.22, 0.4, 3, 10, 0.02),
                (PlaylistType.Curated, TimeOfDay.Evening)   => new(3, 0.45, 0.25, 0.4, 3, 10, 0.02),
                (PlaylistType.Curated, TimeOfDay.Late)      => new(3, 0.35, 0.28, 0.4, 3, 10, 0.02),

                (PlaylistType.Normal,  TimeOfDay.Morning)   => new(10, 0.80, 0.18, 0.5, 3, 8, 0.10),
                (PlaylistType.Normal,  TimeOfDay.Afternoon) => new(10, 0.80, 0.18, 0.5, 3, 8, 0.10),
                (PlaylistType.Normal,  TimeOfDay.Evening)   => new(8,  0.70, 0.18, 0.5, 3, 8, 0.10),
                (PlaylistType.Normal,  TimeOfDay.Late)      => new(6,  0.60, 0.18, 0.5, 3, 8, 0.10),

                _ => throw new ArgumentOutOfRangeException()
            };
    }
}