using Microsoft.VisualBasic.FileIO;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
        /// Import the contents of the specified CSV file
        /// </summary>
        /// <param name="file"></param>
        public async Task Import(string file)
        {
            // Create a text parser to read the CSV file
            using (TextFieldParser parser = new(file, Encoding.UTF8))
            {
                // Configure the delimiters
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Read to the end of the file
                int count = 0;
                while (!parser.EndOfData)
                {
                    count++;
                    try
                    {
                        // Read the next row and make sure it's valid
                        var fields = parser.ReadFields();

                        // Ignore the headers in the first line
                        if (count > 1)
                        {
                            // Inflate the CSV record to a track and save the artist
                            FlattenedTrack track = FlattenedTrack.FromCsv(fields);
                            var artist = await _factory.Artists.AddAsync(track.ArtistName);

                            // See if the album exists
                            var album = await _factory.Albums.AddAsync(artist.Id, track.AlbumTitle, track.Released, track.Genre, track.CoverUrl);
                            await _factory.Tracks.AddAsync(album.Id, track.Title, track.TrackNumber, track.Duration);

                            TrackImport?.Invoke(this, new TrackDataExchangeEventArgs { RecordCount = count - 1, Track = track });

                        }
                    }
                    catch (Exception ex)
                    {
                        string message = $"Invalid record format at line {count} of {file} : {ex.Message}";
                        throw new InvalidRecordFormatException(message);
                    }
                }
            }
        }
    }
}
