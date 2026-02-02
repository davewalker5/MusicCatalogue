using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public record PlaylistPresetBase
    (
        int TopK,
        double Temperature,
        double TransitionPenalty,
        double Variety,
        int MinimumEntries,
        int AvoidRecent,
        double RandomJitter
    );
}