using MusicCatalogue.Entities.Database;
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

        public const string CsvRecordPattern = @"^(""[a-zA-Z0-9-() \/']+"",){3}""[0-9]+"",("".*"",)""[0-9]+"",(""[a-zA-Z0-9-() \/']+"",)""[0-9]+\:[0-9]{2}""$";

        public string ArtistName{ get; set; } = "";
        public string AlbumTitle { get; set; } = "";
        public string? Genre { get; set; } = "";
        public int? Released { get; set; }
        public string? CoverUrl { get; set; } = "";
        public int? TrackNumber { get; set; }
        public string Title { get; set; } = "";

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
            return builder.ToString();
        }

        /// <summary>
        /// Create a flattened track record from a CSV string
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static FlattenedTrack FromCsv(string record)
        {
            // Split the record into words
            var words = record.Split(new string[] { "\",\"" }, StringSplitOptions.None);

            // Get the release date and cover URL, both of which may be NULL
            int? releaseYear = !string.IsNullOrEmpty(words[ReleasedField]) ? int.Parse(words[ReleasedField]) : null;
            string? coverUrl = !string.IsNullOrEmpty(words[CoverField]) ? words[CoverField] : null;

            // Split the duration on the ":" separator and convert to milliseconds
            var durationWords = words[DurationField][..^1].Split(new string[] { ":" }, StringSplitOptions.None);
            var durationMs = 1000 * (60 * int.Parse(durationWords[0]) +  int.Parse(durationWords[1]));
 
            // Create a new "flattened" record containing artist, album and track details
            return new FlattenedTrack
            {
                ArtistName = words[ArtistField][1..],
                AlbumTitle = words[AlbumField],
                Genre = words[GenreField],
                Released = releaseYear,
                CoverUrl = coverUrl,
                TrackNumber = int.Parse(words[TrackNumberField]),
                Title = words[TitleField],
                Duration = durationMs
            };
        }

        /// <summary>
        /// Append a value to a string builder holding a representation of a flattened track in CSV format
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        private static void AppendField(StringBuilder builder, object? value)
        {
            if (builder.Length > 0)
            {
                builder.Append(',');
            }

            builder.Append('"');
            builder.Append(value?.ToString() ?? "");
            builder.Append('"');
        }
    }
}
