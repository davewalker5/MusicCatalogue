using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicCatalogue.Logic.DataExchange
{
    [ExcludeFromCodeCoverage]
    public partial class CsvImporter : DataExchangeBase, IImporter
    {
        public event EventHandler<TrackDataExchangeEventArgs>? TrackImport;

#pragma warning disable CS8618
        internal CsvImporter(IMusicCatalogueFactory factory) : base(factory)
        {
        }
#pragma warning restore CS8618

        /// <summary>
        /// Regular expression used to validate CSV record format
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("^(\"[a-zA-Z0-9-() \\/']+\",){3}\"[0-9]+\",(\".*\",)\"[0-9]+\",(\"[a-zA-Z0-9-() \\/']+\",)\"[0-9]+\\:[0-9]{2}\"$", RegexOptions.Compiled)]
        private static partial Regex RecordFormatRegex();

        /// <summary>
        /// Import the contents of the specified CSV file
        /// </summary>
        /// <param name="file"></param>
        public async Task Import(string file)
        {
            Regex regex = RecordFormatRegex();

            using (StreamReader reader = new(file, Encoding.UTF8))
            {
                int count = 0;
                while (!reader.EndOfStream)
                {
                    // Read the next line and make sure it's got some content
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        // Increment the record cound
                        count++;
                        if (count > 1)
                        {
                            // Check the line matches the pattern required for successful import. Note that this does
                            // not allow commas in the artist name, title or track name
                            bool matches = regex.Matches(line!).Any();
                            if (!matches)
                            {
                                Console.WriteLine(line);
                                string message = $"Invalid record format at line {count} of {file}";
                                throw new InvalidRecordFormatException(message);
                            }

                            // Inflate the CSV record to a track and save the artist
                            FlattenedTrack track = FlattenedTrack.FromCsv(line);
                            var artist = await _factory.Artists.AddAsync(track.ArtistName);

                            // See if the album exists
                            var album = await _factory.Albums.AddAsync(artist.Id, track.AlbumTitle, track.Released, track.Genre, track.CoverUrl);
                            await _factory.Tracks.AddAsync(album.Id, track.Title, track.TrackNumber, track.Duration);

                            TrackImport?.Invoke(this, new TrackDataExchangeEventArgs { RecordCount = count - 1, Track = track });
                        }
                    }
                }
            }
        }
    }
}
