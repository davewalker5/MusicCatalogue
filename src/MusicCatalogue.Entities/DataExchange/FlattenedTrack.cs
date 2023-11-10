using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MusicCatalogue.Entities.DataExchange
{
    [ExcludeFromCodeCoverage]
    public class FlattenedTrack : TrackBase
    {
        public const int ArtistField = 0;
        public const int AlbumField = 1;
        private const int GenreField = 2;
        private const int ReleasedField = 3;
        private const int CoverField = 4;
        private const int TrackNumberField = 5;
        private const int TitleField = 6;
        private const int DurationField = 7;
        private const int WishlistItemField = 8;

        public string ArtistName{ get; set; } = "";
        public string AlbumTitle { get; set; } = "";
        public string? Genre { get; set; } = "";
        public int? Released { get; set; }
        public string? CoverUrl { get; set; } = "";
        public int? TrackNumber { get; set; }
        public string Title { get; set; } = "";
        public bool? IsWishlistItem { get; set;  }

        /// <summary>
        /// Create a representation of the flattened track in CSV format
        /// </summary>
        /// <returns></returns>
        public string ToCsv()
        {
            StringBuilder builder = new StringBuilder();
            AppendField(builder, ArtistName);
            AppendField(builder, AlbumTitle);
            AppendField(builder, Genre);
            AppendField(builder, Released);
            AppendField(builder, CoverUrl);
            AppendField(builder, TrackNumber);
            AppendField(builder, Title);
            AppendField(builder, FormattedDuration);
            AppendField(builder, (IsWishlistItem ?? false).ToString());
            return builder.ToString();
        }

        /// <summary>
        /// Create a flattened track record from a CSV string
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static FlattenedTrack FromCsv(IList<string> fields)
        {
            // Check we have the required number of fields
            if ((fields == null) || (fields.Count != 9))
            {
                throw new InvalidRecordFormatException("Incorrect number of CSV fields");
            }

            // Get the release date and cover URL, both of which may be NULL
            int? releaseYear = !string.IsNullOrEmpty(fields[ReleasedField]) ? int.Parse(fields[ReleasedField]) : null;
            string? coverUrl = !string.IsNullOrEmpty(fields[CoverField]) ? fields[CoverField] : null;

            // Split the duration on the ":" separator and convert to milliseconds
            var durationWords = fields[DurationField].Split(new string[] { ":" }, StringSplitOptions.None);
            var durationMs = 1000 * (60 * int.Parse(durationWords[0]) +  int.Parse(durationWords[1]));
 
            // Create a new "flattened" record containing artist, album and track details
            return new FlattenedTrack
            {
                ArtistName = fields[ArtistField],
                AlbumTitle = fields[AlbumField],
                Genre = fields[GenreField],
                Released = releaseYear,
                CoverUrl = coverUrl,
                TrackNumber = int.Parse(fields[TrackNumberField]),
                Title = fields[TitleField],
                Duration = durationMs,
                IsWishlistItem = bool.Parse(fields[WishlistItemField])
            };
        }

        /// <summary>
        /// Append a value to a string builder holding a representation of a flattened track in CSV format
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        private static void AppendField(StringBuilder builder, object? value)
        {
            // Add a separator if there are already fields in the line under construction
            if (builder.Length > 0)
            {
                builder.Append(',');
            }

            // Convert the value to string and see if it contains the delimiter
            var stringValue = (value?.ToString() ?? "").Replace('"', '\'');
            var containsDelimiter = !string.IsNullOrEmpty(stringValue) && stringValue.Contains(',');

            // Add the value to the builder, quoting it if needed
            if (containsDelimiter) builder.Append('"');
            builder.Append(stringValue);
            if (containsDelimiter) builder.Append('"');
        }
    }
}
