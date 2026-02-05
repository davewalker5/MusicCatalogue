using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public record PlaylistParameters(
        TimeOfDay TimeOfDay,
        int? CurrentArtistId,
        int NumberOfEntries,
        int TopK,
        double Temperature,
        double TransitionPenalty,
        double StyleWeight,
        double MoodWeight,
        int AvoidRecent,
        double RandomJitter,
        int? Seed
    );
}