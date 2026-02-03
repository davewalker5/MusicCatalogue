using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;
using Spectre.Console;

namespace MusicCatalogue.LookupTool.Logic
{
    internal class PlaylistGenerator
    {
        private readonly IMusicCatalogueFactory _factory;

        public PlaylistGenerator(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Generate a playlist and show it on the console
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="numberOfEntries"></param>
        /// <returns></returns>
        public async Task GeneratePlaylistAsync(PlaylistType type, TimeOfDay timeOfDay, int numberOfEntries)
        {
            // Create a playlist
            var albums = await CreatePlaylistAsync(type, timeOfDay, numberOfEntries);

            // Create a table and add the columns to it
            var table = new Table();
            table.AddColumn("#");
            table.AddColumn("Artist");
            table.AddColumn("Album");
            table.AddColumn("Playing Time");

            // Add the albums to the table
            for (int i = 0; i < albums.Count; i++)
            {
                var rowData = new string[] {
                    GetCellData(i + 1),
                    GetCellData(albums[i].Artist!.Name),
                    GetCellData(albums[i].Title),
                    GetCellData(albums[i].FormattedPlayingTime)
                };

                table.AddRow(rowData);
            }

            // Add the total playing time row
            var totalPlayingTime = FormattedTotalPlayingTime(albums);
            var totalPlayingTimeRow = new string[] {
                    GetCellData(""),
                    GetCellData(""),
                    GetCellData(""),
                    GetCellData(totalPlayingTime)
                };
            table.AddRow(totalPlayingTimeRow);

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// Generate a playlist and export it to a CSV or XLSX file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="numberOfEntries"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task ExportPlaylistAsync(PlaylistType type, TimeOfDay timeOfDay, int numberOfEntries, string filePath)
        {
            // Create a playlist
            var albums = await CreatePlaylistAsync(type, timeOfDay, numberOfEntries);

            // Determine the export file type and get the appropriate exporter
            var exporter = Path.GetFileName(filePath).EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ?
                _factory.PlaylistXlsxExporter : _factory.PlaylistCsvExporter;

            // Export the playlist
            exporter.Export(filePath, albums);
        }

        /// <summary>
        /// Get the content for a table cell
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetCellData(object value)
            => $"[white]{value}[/]";

        /// <summary>
        /// Create a playlist
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="numberOfEntries"></param>
        /// <returns></returns>
        private async Task<List<Album>> CreatePlaylistAsync(PlaylistType type, TimeOfDay timeOfDay, int numberOfEntries)
        {
            var playlist = await _factory.ArtistPlaylistBuilder.BuildPlaylist(type, timeOfDay, numberOfEntries);
            var albums = await _factory.ArtistPlaylistBuilder.PickPlaylistAlbums(playlist);
            return albums;
        }

        /// <summary>
        /// Calculate the total playing time for a playlist
        /// </summary>
        /// <param name="albums"></param>
        /// <returns></returns>
        private string FormattedTotalPlayingTime(List<Album> albums)
        {
       
            var totalPlayingTime = albums.Sum(x => x.PlayingTime);
            int seconds = totalPlayingTime / 1000;
            int hours = seconds / 3600;
            seconds -= 3600 * hours;
            int minutes = seconds / 60;
            seconds -= 60 * minutes;
            return $"{hours:00}:{minutes:00}:{seconds:00}";
        }
    }
}