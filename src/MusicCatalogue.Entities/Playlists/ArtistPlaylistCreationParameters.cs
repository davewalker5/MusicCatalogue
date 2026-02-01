using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Entities.Playlists
{
    [ExcludeFromCodeCoverage]
    public class ArtistPlaylistCreationParameters : MusicCatalogueEntityBase
    {
        /// <summary>
        /// Number of entries in the playlist
        /// </summary>
        public int NumberOfEntries { get; set; }

        /// <summary>
        /// Used to weight artist style when calculating the artist score
        /// </summary>
        public double StyleWeight { get; set; }

        /// <summary>
        /// Used to weight the artists moods when calculating the artist score
        /// </summary>
        public double MoodWeight { get; set; }

        /// <summary>
        /// Guides the flow between artists in the list
        /// </summary>
        public double TransitionPenalty { get; set; }

        /// <summary>
        /// Number of artists to retain in the "recent" list to avoid repetition
        /// </summary>
        public int AvoidRecent { get; set; }

        /// <summary>
        /// How many artists are allowed to compete for each spot in the list
        /// </summary>
        public int TopK { get; set; }

        /// <summary>
        /// How random the choise is within candidates - below 0.3, it becomes almost deterministic (top score
        /// is always chosen) and above 0.85 it becomes too loose
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Small random element added to the score for a given candidate
        /// </summary>
        public double RandomJitter { get; set; }

        /// <summary>
        /// Used to seed the random number generator
        /// </summary>
        public int? Seed { get; set; }

        /// <summary>
        /// Time of day to target
        /// </summary>
        public TimeOfDay TimeOfDay { get; set; }
    }
}